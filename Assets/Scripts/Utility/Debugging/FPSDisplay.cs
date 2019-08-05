using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ATT.Utility.Debugging
{
    /// <summary>
    /// Calculates and displays frames per second.
    /// Based on VRTK_FramesPerSecondViewer.cs
    /// </summary>
    public class FPSDisplay : MonoBehaviour
    {
        [Tooltip("Toggles whether the FPS text is visible.")]
        public bool ShouldDisplay = true;
        [Tooltip("The frames per second deemed acceptable that is used as the benchmark to change the FPS text colour.")]
        public int TargetFPS = 90;
        [Tooltip("The colour of the FPS text when the frames per second are within reasonable limits of the Target FPS.")]
        public Color GoodColor = Color.green;
        [Tooltip("The colour of the FPS text when the frames per second are falling short of reasonable limits of the Target FPS.")]
        public Color WarningColor = Color.yellow;
        [Tooltip("The colour of the FPS text when the frames per second are at an unreasonable level of the Target FPS.")]
        public Color BadColor = Color.red;

        private const float updateInterval = 0.5f;
        private int framesCount;
        private float framesTime;

        #region References

        Text _fpsText;

        #endregion

        #region Unity Methods

        void Start()
        {
            SetAndCheckReferences();
        }

        void Update()
        {
            if (ShouldDisplay)
            {
                DisplayFPS();
            }
        }
        #endregion

        private void DisplayFPS()
        {
            framesCount++;
            framesTime += Time.unscaledDeltaTime;

            if (framesTime > updateInterval)
            {
                var fps = framesCount / framesTime;
                _fpsText.text = $"{fps:F2} FPS";
                _fpsText.material.color = (fps > (TargetFPS - 5) ? GoodColor : (fps > (TargetFPS - 30) ? WarningColor : BadColor));
                _fpsText.color = _fpsText.material.color;//(fps > (TargetFPS - 5) ? GoodColor : (fps > (TargetFPS - 30) ? WarningColor : BadColor));
                framesCount = 0;
                framesTime = 0;
            }
        }

        #region Debugging
        private void SetAndCheckReferences()
        {
            _fpsText = GetComponent<Text>();
            Assert.IsNotNull(_fpsText, $"{gameObject.name} has no reference to the Text component.");
        }
        #endregion
    }
}