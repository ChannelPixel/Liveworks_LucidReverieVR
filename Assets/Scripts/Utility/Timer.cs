using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Utility
{
    public class Timer : MonoBehaviour
    {
        public enum TimeState { None, Good, Warning, Critical, RanOut, Stopped }
        
        [Header("Time Values")]
        [SerializeField] private float countdownValue = 0;
        [SerializeField] private float totalTime = 0;

        [Header("State Change Ratios")]
        [SerializeField]
        private float warningRatio = 0.5f;
        [SerializeField]
        private float criticalRatio = 0.15f;

        [Header("Debugging")]
        [SerializeField]
        private TimeState countdownState = TimeState.None;
        [SerializeField]
        private bool countdownIsPaused = false;


        private float countdownStart = 0;
        private float updateInterval = 1;
        private bool keepTotalTime = true;
        
        WaitForSecondsRealtime updateSeconds;

        private Coroutine countUpRoutine;
        private Coroutine countdownRoutine;

        public event Action<string> OnCountdownValueChanged;
        public event Action<TimeState> OnCountdownStateChanged;
        public event Action OnCountdownStarted;
        public event Action OnCountdownReachedWarning;
        public event Action OnCountdownReachedCritical;
        public event Action OnCountdownEnded;

        #region Unity Methods
        void Start()
        {
            updateSeconds = new WaitForSecondsRealtime(updateInterval);

            StartCountUpTimer();
        }

        void OnApplicationQuit()
        {
            Debug.Log($"<b>[Timer]</b> Total play time: {GetFormattedTimeString(totalTime)}");

            if (countdownValue > 0)
            {
                Debug.Log($"<b>[Timer]</b> Countdown value: {GetFormattedTimeString(countdownValue)}");
            }
        }
        #endregion

        #region Total Time
        private void StartCountUpTimer()
        {
            countUpRoutine = StartCoroutine(CountUp());
        }

        public void StopCountUpTimer()
        {
            if (countUpRoutine == null) return;
            StopCoroutine(countUpRoutine);
            countUpRoutine = null;
        }

        IEnumerator CountUp()
        {
            while (keepTotalTime)
            {
                yield return updateSeconds;
                totalTime++;
            }
        }
        #endregion

        #region Countdown Timer
        public void StartCountdownTimer(float startValue)
        {
            Assert.IsTrue(startValue > 0f, $"<b>[Timer]</b> Countdown timer must start with a value greater than zero (set to {startValue}).");
            if (countdownRoutine == null)
            {
                countdownStart = startValue;
                countdownRoutine = StartCoroutine(CountDown(countdownStart));
                OnCountdownStarted?.Invoke();
            }
        }
        
        public void PauseCountdownTimer()
        {
            countdownIsPaused = true;
            Debug.Log($"Countdown paused. Value: {countdownValue}");
        }

        public void ResumeCountdownTimer()
        {
            countdownIsPaused = false;
            Debug.Log($"Countdown resumed. Value: {countdownValue}");
        }


        public void StopCountdownTimer()
        {
            if (countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
                countdownRoutine = null;
            }
        }

        //Todo
        private void ResetCountdownTimer() { }


        IEnumerator CountDown(float startSeconds)
        {
            StopCountUpTimer();

            Debug.Log($"<b>[Timer]</b> Countdown started @ {GetFormattedTimeString(totalTime)}.");

            countdownValue = startSeconds;

            while (countdownValue > 0)
            {
                yield return updateSeconds;

                if (countdownIsPaused)
                {
                    yield return null;
                }
                else
                {
                    countdownValue--;
                    SetCountdownStateAndNotify();
                    if (keepTotalTime)
                    {
                        totalTime++;
                    }
                }
            }

            Debug.Log($"<b>[Timer]</b> Countdown ran out @ {GetFormattedTimeString(totalTime)}.");
            yield break;
        }

        private void SetCountdownStateAndNotify()
        {
            var state = (countdownValue > (countdownStart * warningRatio)) ? TimeState.Good
                : (countdownValue > (countdownStart * criticalRatio)) ? TimeState.Warning
                : (countdownValue > 0f) ? TimeState.Critical : TimeState.RanOut;

            if (state != countdownState)
            {
                switch (state)
                {
                    case TimeState.Warning:
                        OnCountdownReachedWarning?.Invoke();
                        break;
                    case TimeState.Critical:
                        OnCountdownReachedCritical?.Invoke();
                        break;
                    case TimeState.RanOut:
                        StopCoroutine(countdownRoutine);
                        countdownValue = 0;
                        OnCountdownEnded?.Invoke();
                        break;
                    case TimeState.None:
                    case TimeState.Good:
                    case TimeState.Stopped:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                countdownState = state;
                OnCountdownStateChanged?.Invoke(countdownState);
            }

            OnCountdownValueChanged?.Invoke(GetFormattedTimeString(countdownValue));
        }
        #endregion
        
        private static string GetFormattedTimeString(float timeValue)
        {
            return $"{Mathf.Floor(timeValue / 60):00}:{Mathf.Floor(timeValue % 60):00}";
        }
    }
}