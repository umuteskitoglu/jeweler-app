using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Services;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class IyzicoPaymentService : IPaymentService
{
    private readonly IyzicoSettings _settings;
    private readonly HttpClient _httpClient;

    public IyzicoPaymentService(IOptions<IyzicoSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    public async Task<PaymentResult> InitiatePaymentAsync(Order order, string paymentMethod, string callbackUrl)
    {
        try
        {
            var request = new
            {
                locale = "tr",
                conversationId = order.Id.ToString(),
                price = order.TotalAmount.Amount.ToString("F2"),
                paidPrice = order.TotalAmount.Amount.ToString("F2"),
                currency = order.TotalAmount.Currency,
                basketId = order.Id.ToString(),
                paymentGroup = "PRODUCT",
                callbackUrl = callbackUrl,
                enabledInstallments = new[] { 1 },
                buyer = new
                {
                    id = order.CustomerId.ToString(),
                    name = "Customer",
                    surname = "User",
                    identityNumber = "11111111111",
                    email = "customer@example.com",
                    registrationAddress = "Address",
                    city = "Istanbul",
                    country = "Turkey",
                    ip = "85.34.78.112"
                },
                shippingAddress = new
                {
                    address = "Shipping Address",
                    contactName = "Customer User",
                    city = "Istanbul",
                    country = "Turkey"
                },
                billingAddress = new
                {
                    address = "Billing Address",
                    contactName = "Customer User",
                    city = "Istanbul",
                    country = "Turkey"
                },
                basketItems = order.Items.Select(item => new
                {
                    id = item.ProductId.ToString(),
                    name = item.ProductName,
                    category1 = "Jewelry",
                    itemType = "PHYSICAL",
                    price = (item.UnitPrice.Amount * item.Quantity).ToString("F2")
                }).ToArray()
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var randomString = Guid.NewGuid().ToString();
            var authString = GenerateAuthorizationHeader("/payment/iyzipos/checkoutform/initialize/auth/ecom", jsonRequest, randomString);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", authString);
            _httpClient.DefaultRequestHeaders.Add("x-iyzi-rnd", randomString);

            var response = await _httpClient.PostAsync("/payment/iyzipos/checkoutform/initialize/auth/ecom", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IyzicoInitializeResponse>(responseContent);

            if (result?.status == "success")
            {
                return new PaymentResult(true, result.paymentPageUrl, result.token, null);
            }

            return new PaymentResult(false, null, null, result?.errorMessage ?? "Payment initialization failed");
        }
        catch (Exception ex)
        {
            return new PaymentResult(false, null, null, ex.Message);
        }
    }

    public async Task<PaymentResult> ProcessPaymentCallbackAsync(string token, string conversationId)
    {
        try
        {
            var request = new
            {
                locale = "tr",
                conversationId = conversationId,
                token = token
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var randomString = Guid.NewGuid().ToString();
            var authString = GenerateAuthorizationHeader("/payment/iyzipos/checkoutform/auth/ecom/detail", jsonRequest, randomString);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", authString);
            _httpClient.DefaultRequestHeaders.Add("x-iyzi-rnd", randomString);

            var response = await _httpClient.PostAsync("/payment/iyzipos/checkoutform/auth/ecom/detail", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IyzicoCallbackResponse>(responseContent);

            if (result?.status == "success" && result?.paymentStatus == "SUCCESS")
            {
                return new PaymentResult(true, null, result.paymentId, null);
            }

            return new PaymentResult(false, null, null, result?.errorMessage ?? "Payment verification failed");
        }
        catch (Exception ex)
        {
            return new PaymentResult(false, null, null, ex.Message);
        }
    }

    public async Task<bool> RefundPaymentAsync(Payment payment, Money amount)
    {
        try
        {
            var request = new
            {
                locale = "tr",
                conversationId = payment.OrderId.ToString(),
                paymentTransactionId = payment.TransactionId,
                price = amount.Amount.ToString("F2"),
                currency = amount.Currency,
                ip = "85.34.78.112"
            };

            var jsonRequest = JsonSerializer.Serialize(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var randomString = Guid.NewGuid().ToString();
            var authString = GenerateAuthorizationHeader("/payment/refund", jsonRequest, randomString);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", authString);
            _httpClient.DefaultRequestHeaders.Add("x-iyzi-rnd", randomString);

            var response = await _httpClient.PostAsync("/payment/refund", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IyzicoRefundResponse>(responseContent);

            return result?.status == "success";
        }
        catch
        {
            return false;
        }
    }

    private string GenerateAuthorizationHeader(string uri, string requestBody, string randomString)
    {
        var dataToEncrypt = $"{_settings.ApiKey}{randomString}{_settings.SecretKey}{requestBody}";
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(dataToEncrypt));
        var hashString = Convert.ToBase64String(hash);
        return $"IYZWS {_settings.ApiKey}:{hashString}";
    }

    private class IyzicoInitializeResponse
    {
        public string? status { get; set; }
        public string? paymentPageUrl { get; set; }
        public string? token { get; set; }
        public string? errorMessage { get; set; }
    }

    private class IyzicoCallbackResponse
    {
        public string? status { get; set; }
        public string? paymentStatus { get; set; }
        public string? paymentId { get; set; }
        public string? errorMessage { get; set; }
    }

    private class IyzicoRefundResponse
    {
        public string? status { get; set; }
    }
}

