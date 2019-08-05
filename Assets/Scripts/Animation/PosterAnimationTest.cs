using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Animation
{
    public class PosterAnimationTest : MonoBehaviour
    {
        private const string smileTrigger = "Smile";
        private const string mouthlessTrigger = "DEBUG_Mouthless";

        [Header("Debugging")] public bool AllowDebug = true;
        public KeyCode SmileKey = KeyCode.S;
        public KeyCode MouthlessKey = KeyCode.M;


        #region References

        Animator _animator;

        #endregion

        void Start()
        {
            SetAndCheckReferences();
        }

        void Update()
        {
            if (AllowDebug)
            {
                HandleKeyboardInput();
            }
        }

        private void HandleKeyboardInput()
        {
            if (Input.GetKeyDown(SmileKey))
            {
                _animator.SetTrigger(smileTrigger);
            }

            if (Input.GetKeyDown(MouthlessKey))
            {
                _animator.SetTrigger(mouthlessTrigger);
            }
        }

        private void SetAndCheckReferences()
        {
            _animator = GetComponent<Animator>();
            Assert.IsNotNull(_animator, $"[PosterAnimationTest] {gameObject.name} has no animator component.");
        }
    }
}