using System.Globalization;

namespace Jewelry.API.Middleware;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var culture = context.Request.Headers["Accept-Language"].FirstOrDefault() 
                     ?? context.Request.Query["lang"].FirstOrDefault() 
                     ?? "tr-TR";

        var supportedCultures = new[] { "tr-TR", "en-US" };
        
        // Normalize culture
        if (!supportedCultures.Contains(culture))
        {
            // Try to match by language only (e.g., "tr" -> "tr-TR")
            var languageOnly = culture.Split('-')[0];
            culture = supportedCultures.FirstOrDefault(c => c.StartsWith(languageOnly)) ?? "tr-TR";
        }

        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}

