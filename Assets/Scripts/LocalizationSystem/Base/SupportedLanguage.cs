using System;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Base
{
    [Serializable]
    public class SupportedLanguage
    {
        [SerializeField]
        private SystemLanguage _language;
        [SerializeField]
        private string _resourceFileName;

        public SystemLanguage Language => _language;
        public string ResourceFileName => _resourceFileName;

        public SupportedLanguage(SystemLanguage lang)
        {
            _language = lang;
        }

        public void SetResourceFileName(string name)
        {
            _resourceFileName = name;
        }
    }
}

