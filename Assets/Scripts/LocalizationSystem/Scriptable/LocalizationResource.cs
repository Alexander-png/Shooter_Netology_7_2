using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Scriptable
{
    [CreateAssetMenu(menuName = "Localization/Resource", fileName = "localization.asset")]
    public class LocalizationResource : ScriptableObject
    {
        [SerializeField]
        private bool _isRTL;
        [SerializeField]
        private TMP_FontAsset _font;
        [SerializeField]
        private List<LocalizationTerm> _terms;

        public bool IsEmpty => _terms == null || _terms.Count == 0;

        public bool IsRTL => _isRTL;
        public TMP_FontAsset Font => _font;
        public List<LocalizationTerm> Terms => new List<LocalizationTerm>(_terms);

        public void SetTerms(List<LocalizationTerm> newTerms)
        {
            _terms = newTerms;
        }
    }

    [Serializable]
    public struct LocalizationTerm
    {
        public string Key;
        public string Value;
    }
}