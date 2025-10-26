using System.Globalization;
using System.Text.Json;
using Application.Localization;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly Dictionary<string, Dictionary<string, object>> _localizations;
    private readonly string _defaultCulture = "tr-TR";

    public LocalizationService(IHostEnvironment environment)
    {
        _localizations = new Dictionary<string, Dictionary<string, object>>();
        LoadLocalizations(environment.ContentRootPath);
    }

    private void LoadLocalizations(string contentRootPath)
    {
        var localizationPath = Path.Combine(contentRootPath, "Infrastructure", "Localization", "Resources");
        
        // If running from API project, adjust path
        if (!Directory.Exists(localizationPath))
        {
            localizationPath = Path.Combine(Directory.GetParent(contentRootPath)!.FullName, "Infrastructure", "Localization", "Resources");
        }

        var supportedCultures = new[] { "tr-TR", "en-US" };

        foreach (var culture in supportedCultures)
        {
            var filePath = Path.Combine(localizationPath, $"{culture}.json");
            
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                
                if (data != null)
                {
                    _localizations[culture] = data;
                }
            }
        }
    }

    public string GetString(string key)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        
        // Try exact culture first
        if (!_localizations.ContainsKey(culture))
        {
            // Try language only (e.g., "tr" from "tr-TR")
            var languageOnly = culture.Split('-')[0];
            culture = _localizations.Keys.FirstOrDefault(k => k.StartsWith(languageOnly)) ?? _defaultCulture;
        }

        if (!_localizations.ContainsKey(culture))
        {
            culture = _defaultCulture;
        }

        // Support nested keys like "User.RegisteredSuccessfully"
        var keys = key.Split('.');
        object? current = _localizations[culture];

        foreach (var k in keys)
        {
            if (current is JsonElement element)
            {
                if (element.TryGetProperty(k, out var property))
                {
                    current = property;
                }
                else
                {
                    return key; // Key not found, return the key itself
                }
            }
            else if (current is Dictionary<string, object> dict)
            {
                if (dict.TryGetValue(k, out var value))
                {
                    current = value;
                }
                else
                {
                    return key;
                }
            }
            else
            {
                return key;
            }
        }

        // Handle JsonElement to string conversion
        if (current is JsonElement finalElement)
        {
            return finalElement.GetString() ?? key;
        }

        return current?.ToString() ?? key;
    }

    public string GetString(string key, params object[] args)
    {
        var format = GetString(key);
        try
        {
            return string.Format(format, args);
        }
        catch
        {
            return format;
        }
    }

    public void SetLanguage(string culture)
    {
        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public string GetCurrentLanguage()
    {
        return CultureInfo.CurrentUICulture.Name;
    }
}

