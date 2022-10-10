using Lesson_7_4.LocalizationSystem.Base;
using Lesson_7_4.LocalizationSystem.Providers.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lesson_7_4.LocalizationSystem.Providers
{
    public class LegacyTextLocalizator : LocalizationProvider
    {
        private Dictionary<string, string> _parameters;

        [SerializeField]
        private string _termKey;
        [SerializeField]
        private Text _textObject;

        public string TermKey
        {
            get => _termKey;
            set
            {
                _termKey = value;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            _textObject.text = LocalizationCore.GetTerm(TermKey, _parameters);
        }

        private void Awake() =>
            Refresh();

        private void OnValidate() =>
            Refresh();

        private void Refresh()
        {
            FindTextIfNoReference();
            UpdateText();
        }

        private void FindTextIfNoReference()
        {
            if (_textObject == null)
            {
                _textObject = GetComponent<Text>();
            }
        }

        public void SetParameters(Dictionary<string, string> newParams)
        {
            _parameters = newParams;
            UpdateText();
        }

        protected override void UpdateValue()
        {
            UpdateText();
        }
    }
}
