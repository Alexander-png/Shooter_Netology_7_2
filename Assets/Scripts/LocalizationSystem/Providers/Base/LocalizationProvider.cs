using Lesson_7_4.LocalizationSystem.Base;
using UnityEngine;

namespace Lesson_7_4.LocalizationSystem.Providers.Base
{
    public abstract class LocalizationProvider : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            LocalizationCore.OnLanguageChanged += UpdateValue;
        }

        protected virtual void OnDisable()
        {
            LocalizationCore.OnLanguageChanged -= UpdateValue;
        }

        protected virtual void UpdateValue() { }
    }
}