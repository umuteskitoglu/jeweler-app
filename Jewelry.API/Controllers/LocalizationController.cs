using Application.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Jewelry.API.Controllers;

/// <summary>
/// Localization operations
/// </summary>
public class LocalizationController : BaseController
{
    private readonly ILocalizationService _localizationService;

    public LocalizationController(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    /// <summary>
    /// Get current language
    /// </summary>
    [HttpGet("current")]
    public ActionResult<object> GetCurrentLanguage()
    {
        return Ok(new
        {
            culture = CultureInfo.CurrentCulture.Name,
            uiCulture = CultureInfo.CurrentUICulture.Name
        });
    }

    /// <summary>
    /// Get all supported languages
    /// </summary>
    [HttpGet("supported")]
    public ActionResult<object> GetSupportedLanguages()
    {
        return Ok(new
        {
            languages = new[]
            {
                new { code = "tr-TR", name = "Türkçe", englishName = "Turkish" },
                new { code = "en-US", name = "English", englishName = "English" }
            }
        });
    }

    /// <summary>
    /// Test localization with a key
    /// </summary>
    [HttpGet("test/{key}")]
    public ActionResult<object> TestLocalization(string key)
    {
        var value = _localizationService.GetString(key);
        return Ok(new
        {
            key,
            value,
            culture = CultureInfo.CurrentCulture.Name
        });
    }
}

