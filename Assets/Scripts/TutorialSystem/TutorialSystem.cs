using Lesson_7_3.TutorialSystem.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Lesson_7_3.TutorialSystem
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField]
        private Canvas _tutorialCanvas;

        [Header("Tutorial UI component prefabs")]
        [SerializeField]
        private TutorialTextBlock _textBlockPrefab;
        
        [SerializeField]
        private TutorialScript[] _scripts;

        private TutorialScript _currentTutorial;
        private int _currentStep;

        private float _lockTimer;

        private bool IsActive => _currentTutorial != null;
        private TutorialStep CurrentStep => _currentTutorial.Steps[_currentStep];
        private TutorialStep NextStep => _currentTutorial.Steps[_currentStep + 1];
        private bool HasNextStep => _currentTutorial.Steps.Length > _currentStep + 1;

        private bool IsLocked => _lockTimer > 0f;

        private TutorialTextBlock _currentTextBlock;

        private void StartTutorial(TutorialEvent @event)
        {
            foreach (var script in _scripts)
            {
                if (script.StartTrigger == @event)
                {
                    _currentTutorial = script;
                    _currentStep = 0;
                    ProcessTutorialCurrentStep();
                    break;
                }
            }
        }

        private void Start()
        {
            Task.Yield();
            _tutorialCanvas = FindObjectOfType<Canvas>();
        }

        private void FinishTutorial()
        {
            _currentTutorial = null;
            _currentStep = 0;
        }

        private void ProcessTutorialEvent(TutorialEvent @event)
        {
            if (NextStep.StartTrigger == @event)
            {
                PlayNextStep();
            }
            if (!HasNextStep)
            {
                FinishTutorial();
            }
        }

        private void PlayNextStep()
        {
            _currentStep++;
            ProcessTutorialCurrentStep();
        }

        private void ProcessTutorialCurrentStep()
        {
            switch (CurrentStep.Action)
            {
                case TutorialAction.ShowText:
                    ShowText(CurrentStep.Data);
                    break;
                case TutorialAction.HintOnUI:
                    ShowHintOnUI(CurrentStep.Data);
                    break;
                case TutorialAction.HintOnObject:
                    ShowHintOnObject(CurrentStep.Data);
                    break;
                case TutorialAction.Wait:
                    Wait(Convert.ToSingle(CurrentStep.Data));
                    break;
                case TutorialAction.Clear:
                    ClearInstantiatedComponents();
                    break;
            }
        }

        private void ShowHintOnObject(string data)
        {
            
        }

        private void ShowHintOnUI(string data)
        {
            
        }

        private void Wait(float v)
        {
            _lockTimer = v;
        }

        private void ClearInstantiatedComponents()
        {
            if (_currentTextBlock != null)
            {
                Destroy(_currentTextBlock.gameObject);
            }
        }

        private void Update()
        {
            if (IsLocked)
            {
                _lockTimer -= Time.unscaledDeltaTime;
            }

            OnEvent(TutorialEvent.Update);
        }

        private void ShowText(string textToShow)
        {
            _currentTextBlock = Instantiate(_textBlockPrefab, _tutorialCanvas.transform);
            _currentTextBlock.SetText(textToShow);
        }

        public void OnEvent(TutorialEvent @event)
        {
            if (IsLocked)
            {
                return;
            }

            if (IsActive)
            {
                ProcessTutorialEvent(@event);
            }
            else
            {
                StartTutorial(@event);
            }
        }
    }
}
