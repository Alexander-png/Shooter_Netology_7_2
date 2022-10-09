using Lesson_7_4.LocalizationSystem.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Scriptable
{
    [CreateAssetMenu(menuName="Localization/Settings", fileName="localization_settings.asset")]
    public class LocalizationSettings : ScriptableObject
    {
        public const string Prefkey = "lang";
        public static SystemLanguage DefaultLanguage = SystemLanguage.English;
        private static LocalizationSettings _instance;
        
        [SerializeField]
        private SupportedLanguage[] _supportedLanguges;

        public static LocalizationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<LocalizationSettings>("Localizations\\localization_settings");
                }
                return _instance;
            }
        }

        private bool IsEmpty => _supportedLanguges.Length == 0;

        public SupportedLanguage[] SupportedLanguges
        {
            get
            {
                if (_supportedLanguges == null)
                {
                    return new SupportedLanguage[0];
                }

                SupportedLanguage[] toReturn = new SupportedLanguage[_supportedLanguges.Length];
                Array.Copy(_supportedLanguges, toReturn, _supportedLanguges.Length);
                return toReturn;
            }
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (IsEmpty)
            {
                CreateDefaultLanguage();
            }
            CheckAllLanguages();
        }

        private void CreateDefaultLanguage()
        {
            DefaultLanguage = Application.systemLanguage;
            _supportedLanguges = new SupportedLanguage[]
            {
                new SupportedLanguage(DefaultLanguage),
            };
        }

        private void CheckAllLanguages()
        {
            HashSet<SystemLanguage> allLanguages = new HashSet<SystemLanguage>();

            foreach(SupportedLanguage lang in _supportedLanguges)
            {
                if (!IsExists(lang.ResourceFileName))
                {
                    lang.SetResourceFileName(CreateNewResource(lang.Language));
                }

                if (allLanguages.Contains(lang.Language))
                {
                    Debug.LogError($"The supproted locale in your settings with language {lang.Language} was found twice. It's not allowed.");
                }
                else
                {
                    allLanguages.Add(lang.Language);
                }
            }
        }

        private string CreateNewResource(SystemLanguage lang)
        {
            var resName = $"loc_{lang.ToString().ToLower()}";

            if (!IsExists(resName))
            {
                UnityEditor.AssetDatabase.CreateAsset
                (CreateInstance<LocalizationResource>(),
                $"Assets/Resources/Localizations/{resName}.asset");
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }

            return resName;
        }

        private bool IsExists(string resourceFile)
            => Resources.Load<LocalizationResource>($"Localizations\\{resourceFile}") != null;

        [ContextMenu("Check all terms")]
        private void ChekAllTerms()
        {
            Dictionary<SystemLanguage, HashSet<string>> keys = new Dictionary<SystemLanguage, HashSet<string>>();
            HashSet<string> uniqueKeys = new HashSet<string>();

            foreach (SupportedLanguage lang in _supportedLanguges)
            {
                LocalizationResource file = 
                    Resources.Load<LocalizationResource>($"Localizations\\{lang.ResourceFileName}");
                if (file.IsEmpty)
                {
                    continue;
                }

                keys[lang.Language] = new HashSet<string>();
                foreach (LocalizationTerm term in file.Terms)
                {
                    uniqueKeys.Add(term.Key);
                    keys[lang.Language].Add(term.Key);
                }
            }

            foreach (KeyValuePair<SystemLanguage, HashSet<string>> langPair in keys)
            {
                HashSet<string> keySet = langPair.Value;
                keySet.SymmetricExceptWith(uniqueKeys);
                if (keySet.Count != 0)
                {
                    foreach (string key in keySet)
                    {
                        Debug.LogWarning($"Key {key} not found in {langPair.Key}");
                    }
                }
            }
        }

        [ContextMenu("Localizations/Import")]
        public void ImportLocalization()
        {
            CSVLocalizationLoader.ImportCSV();
        }

        [ContextMenu("Localizations/Export")]
        public void ExportLocalization()
        {
            CSVLocalizationLoader.ExportCSV(SupportedLanguges);
        }

        private static class CSVLocalizationLoader
        {
            public static void ImportCSV()
            {
                string path = UnityEditor.EditorUtility.OpenFilePanel("Load localization", "", "csv");

                var fileContent = File.ReadAllLines(path);
                var headers = fileContent[0].Split(';');
                var mapping = new Dictionary<string, int>(headers.Length);
                for (int i = 1; i < headers.Length; i++)
                {
                    mapping[headers[i]] = i;
                }

                var dict = new Dictionary<string, Dictionary<string, string>>();
                for (int i = 1; i < fileContent.Length; i++)
                {
                    var line = fileContent[i].Split(';');
                    var key  = line[0];
                    var value = new Dictionary<string, string>();
                    foreach (var map in mapping)
                    {
                        value[map.Key] = line[map.Value];
                    }
                    dict[key] = value;
                }

                for (int i = 1; i < headers.Length; i++)
                {
                    var lang = headers[i];
                    var resource = Resources.Load<LocalizationResource>($"Localizations\\{lang}");

                    var newTerms = new List<LocalizationTerm>(dict.Count);
                    foreach (var kvp in dict)
                    {
                        newTerms.Add(new LocalizationTerm() 
                        {
                            Key = kvp.Key, 
                            Value = kvp.Value[lang] 
                        });
                    }
                    resource.SetTerms(newTerms);
                }
            }

            public static void ExportCSV(SupportedLanguage[] langs)
            {
                string path = UnityEditor.EditorUtility.SaveFilePanel("Save localization", "", "locals.csv", "csv");

                Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
                string[] langFiles = langs.Select(x => x.ResourceFileName).Distinct().ToArray();

                foreach (var langFile in langFiles)
                {
                    LocalizationResource resource = Resources.Load<LocalizationResource>($"Localizations\\{langFile}");
                    if (resource.IsEmpty)
                    {
                        continue;
                    }

                    foreach (LocalizationTerm term in resource.Terms)
                    {
                        if (!dict.TryGetValue(term.Key, out Dictionary<string, string> subDict))
                        {
                            dict[term.Key] = subDict;
                        }
                        subDict[langFile] = term.Value;
                    }
                }

                StringBuilder csv = new StringBuilder();
                csv.Append("key;").AppendLine(string.Join(";", langFiles));

                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in dict)
                {
                    string[] line = new string[langFiles.Length + 1];
                    line[0] = kvp.Key;

                    for (int i = 0; i < langFiles.Length; i++)
                    {
                        var langFile = langFiles[i];
                        if (kvp.Value.ContainsKey(langFile))
                        {
                            line[i + 1] = kvp.Value[langFile];
                        }
                        else
                        {
                            line[i + 1] = string.Empty;
                        }
                    }
                    csv.AppendLine(string.Join(";", line));
                }
                File.WriteAllText(path, csv.ToString(), Encoding.Unicode);
            }
        }
#endif
    }
}