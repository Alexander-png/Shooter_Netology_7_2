using Lesson_7_4.LocalizationSystem.Base;
using UnityEditor;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Editor
{
    public static class LocalizationSwitch
    {
        [MenuItem("Game/Localization/Reload")]
        public static void ReloadLocalization()
        {
            LocalizationCore.Load();
            UpdateSelectionVisual();
        }

        [MenuItem("Game/Localization/Set/English")]
        public static void SetLanguageEnglish()
        {
            LocalizationCore.SetLanguage(SystemLanguage.English);
            UpdateSelectionVisual();
        }

        [MenuItem("Game/Localization/Set/Russian")]
        public static void SetLanguageRussian()
        {
            LocalizationCore.SetLanguage(SystemLanguage.Russian);
            UpdateSelectionVisual();
        }

        private static void UpdateSelectionVisual()
        {
            Menu.SetChecked("Game/Localization/Set/English", LocalizationCore.SelectedLanguage == SystemLanguage.English);
            Menu.SetChecked("Game/Localization/Set/Russian", LocalizationCore.SelectedLanguage == SystemLanguage.Russian);
        }

        [InitializeOnLoadMethod]
        public static void LoadSelectionVisual()
        {
            EditorApplication.delayCall += UpdateSelectionVisual;
        }
    }
}