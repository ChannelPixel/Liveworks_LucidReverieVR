using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.SpecialEffects
{
    public class BunnyClock : MonoBehaviour
    {
        [Header("Hour Hand")]
        public GameObject HourHand = null;
        private float hourHandSpeed = 40;
        private float hourHandAngle = 1;
        
        [Header("Minute Hand")]
        public GameObject MinuteHand;
        private float minuteHandSpeed = 30;
        private float minuteHandAngle = 10;

        [Header("Second Hand")]
        public GameObject SecondHand;
        private float secondHandSpeed = 35;
        private float secondHandAngle = 25;

        private bool shouldStop = false;
        private bool isRotating = false;
        
        Coroutine rotateRoutine = null;

        #region Unity Methods
        void Start()
        {
            SetAndCheckReferences();            
        }
        #endregion

        public void TriggerClockHandRotation()
        {
            shouldStop = false;
            rotateRoutine = StartCoroutine(RotateClockHands());
        }

        public void StopClockHandRotation()
        {
            shouldStop = true;
            if(rotateRoutine != null)
            {
                StopCoroutine(RotateClockHands());
            }
        }

        IEnumerator RotateClockHands()
        {
            isRotating = true;
            while (!shouldStop)
            {
                SecondHand.transform.Rotate(Vector3.forward, secondHandAngle * secondHandSpeed * Time.deltaTime);
                MinuteHand.transform.Rotate(Vector3.forward, minuteHandAngle * minuteHandSpeed * Time.deltaTime);
                HourHand.transform.Rotate(Vector3.forward, hourHandAngle * hourHandSpeed * Time.deltaTime);

                yield return null;
            }

            isRotating = false;
            yield break;
        }
        
        #region Debugging
        private void SetAndCheckReferences()
        {
            Assert.IsNotNull(HourHand, $"<b>[BunnyClock]<b> Hour hand has not been assigned");
            Assert.IsNotNull(MinuteHand, $"<b>[BunnyClock]<b> Minute hand has not been assigned");
            Assert.IsNotNull(SecondHand, $"<b>[BunnyClock]<b> Second hand has not been assigned");
        }
        #endregion
    }
}