namespace Application.Localization;

public interface ILocalizationService
{
    string GetString(string key);
    string GetString(string key, params object[] args);
    void SetLanguage(string culture);
    string GetCurrentLanguage();
}

