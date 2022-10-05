using TMPro;
using UnityEngine;

namespace Lesson_7_3.TutorialSystem.UI
{
    public class TutorialTextBlock : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _displayText;

        public void SetText(string textToSet)
        {
            _displayText.text = textToSet;
        }
    }
}
