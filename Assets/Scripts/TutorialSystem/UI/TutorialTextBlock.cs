using Lesson_7_4.LocalizationSystem.Base;
using Lesson_7_4.LocalizationSystem.Providers.Base;
using TMPro;
using UnityEngine;

namespace Lesson_7_3.TutorialSystem.UI
{
    public class TutorialTextBlock : LocalizationProvider
    {
        private string _termKey;

        [SerializeField]
        private TMP_Text _displayText;

        public string TermKey
        {
            get => _termKey;
            set
            {
                _termKey = value;
                UpdateText();
            }
        }
        
        protected override void UpdateValue() => UpdateText();

        private void UpdateText() =>
            _displayText.text = LocalizationCore.GetTerm(_termKey, null);
    }
}
