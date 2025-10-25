namespace Infrastructure.Configuration;

public class IyzicoSettings
{
    public const string SectionName = "IyzicoSettings";

    public string ApiKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://sandbox-api.iyzipay.com";
}

