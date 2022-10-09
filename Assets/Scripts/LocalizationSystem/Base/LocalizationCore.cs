using Lesson_7_4.LocalizationSystem.Scriptable;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Base
{
    public static class LocalizationCore
    {
        private static ILookup<string, string> _termsMap;
        public static bool IsLoaded => _termsMap != null;
        public static SystemLanguage SelectedLanguage { get; private set; } = SystemLanguage.English;

        public static TMP_FontAsset Font { get; private set; }
        public static bool RTL { get; private set; }

        public static event Action OnLanguageChanged;

        public static void Load()
        {
            string key = PlayerPrefs.GetString(LocalizationSettings.Prefkey, string.Empty);

            if (Enum.TryParse(key, out SystemLanguage lang))
            {
                SelectedLanguage = lang;
            }
            else
            {
                SelectedLanguage = DetectLangauge();
            }
            LoadTerms();
        }

        private static void LoadTerms()
        {
            var language = LocalizationSettings.Instance.SupportedLanguges.First(x => x.Language == SelectedLanguage);
            var resource = Resources.Load<LocalizationResource>($"Localizations\\{language.ResourceFileName}");
            _termsMap = resource.Terms.ToLookup(item => item.Key, item => item.Value);

            Font = resource.Font;
            RTL = resource.IsRTL;

            OnLanguageChanged?.Invoke();
        }

        public static string GetTerm(string termKey, Dictionary<string, string> parameters = null)
        {
            if (string.IsNullOrEmpty(termKey))
            {
                return string.Empty;
            }

            if (!IsLoaded)
            {
                Load();
            }

            var result = _termsMap[termKey].FirstOrDefault();
            if (result != null)
            {
                if (parameters != null && parameters.Count > 0)
                {
                    parameters.Aggregate(result, (current, param) => 
                    current.Replace($"%{param.Key}%", param.Value));
                    
                }
                return result;
            }
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Debug.LogWarning($"{termKey} not found in {SelectedLanguage} cuture");
            }
#endif
            return $"<MISSING: {termKey}>";
        }

        public static SystemLanguage DetectLangauge()
        {
            SystemLanguage systemLang = Application.systemLanguage;
            foreach(var langRes in LocalizationSettings.Instance.SupportedLanguges)
            {
                if (langRes.Language == systemLang)
                {
                    return langRes.Language;
                }
            }
            return LocalizationSettings.DefaultLanguage;
        }

        public static void SetLanguage(SystemLanguage lang)
        {
            PlayerPrefs.SetString(LocalizationSettings.Prefkey, lang.ToString());
            PlayerPrefs.Save();

            Load();
        }
    }
}