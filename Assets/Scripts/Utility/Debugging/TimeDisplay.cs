using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ATT.Utility.Debugging
{
    /// <summary>
    /// Displays the value of the countdown timer <see cref="Timer"/>.
    /// </summary>
    public class TimeDisplay : MonoBehaviour
    {
        [Header("Countdown")]
        [Tooltip("Toggles whether the countdown value is displayed.")]
        public bool DisplayCountdownTime = true;
        [SerializeField]
        private bool countdownStarted = false;
        //[SerializeField]
        //private bool ignoreTimeScale = true;

        [Header("Colours")]
        [Tooltip("The colour of the Timer text when the time is less than the half of the max play time.")]
        public Color Green = Color.green;
        [Tooltip("The colour of the Timer text when the time is between half and three-quarters of the max play time.")]
        public Color Amber = Color.yellow;
        [Tooltip("The colour of the Timer text when the time is greater than three-quarters of the max play time.")]
        public Color Red = Color.red;

        private Timer timer;

        Text _countdownText;

        #region Unity Methods
        void Start()
        {
            SetAndCheckReferences();

            if (DisplayCountdownTime)
            {
                ObserveTimer();
            }
            
        }
        #endregion

        private void ObserveTimer()
        {
            timer.OnCountdownStarted += ObserveTimer;
            timer.OnCountdownValueChanged += (value) => _countdownText.text = value;
            timer.OnCountdownStateChanged += UpdateTextColour;
        }

        private void UpdateTextColour(Timer.TimeState state)
        {
            switch (state)
            {
                case Timer.TimeState.Good:
                    _countdownText.material.color = Green;
                    _countdownText.color = Green;
                    break;
                case Timer.TimeState.Warning:
                    _countdownText.color = Amber;
                    _countdownText.material.color = Amber;
                    break;
                case Timer.TimeState.Critical:
                case Timer.TimeState.RanOut:
                    _countdownText.color = Red;
                    _countdownText.material.color = Red;
                    break;
                case Timer.TimeState.Stopped:
                    _countdownText.material.color = Color.black;
                    _countdownText.color = Color.black;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        #region Debugging
        private void SetAndCheckReferences()
        {
            _countdownText = GetComponent<Text>();
            Assert.IsNotNull(_countdownText, $"<b>[TimeDisplay]</b> has no Text component.");

            timer = FindObjectOfType<Timer>();
            Assert.IsNotNull(timer, $"<b>[TimeDisplay]</b> Timer script cannot be found.");
        }
        #endregion
    }
}