using System;
using UnityEngine;

namespace Lesson_7_3.TutorialSystem
{
    [CreateAssetMenu(fileName = "Tutorial.asset", menuName = "Tutorial/Create new tutorial script")]
    public class TutorialScript : ScriptableObject
    {
        public TutorialEvent StartTrigger;
        public TutorialStep[] Steps;
    }

    [Serializable]
    public class TutorialStep
    {
        public TutorialEvent StartTrigger;
        public TutorialAction Action;
        public string Data;
    }

    [Serializable]
    public enum TutorialEvent : byte
    {
        Update,
        GameStart,
        PlayerStepMove,
        PlayerMouseMove,
        PlayerJump,
        PlayerSprint,
        PlayerWeaponFire,
        PlayerWeaponReload,
        PlayerWeaponSwitch,
        EnemyDie,
    }

    [Serializable]
    public enum TutorialAction : byte
    {
        ShowText,
        HintOnUI,
        HintOnObject,
        Clear,
        Wait,
    }
}