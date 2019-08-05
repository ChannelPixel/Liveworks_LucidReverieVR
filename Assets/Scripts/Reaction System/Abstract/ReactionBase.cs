using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.ReactionSystem
{
    /// <summary>
    /// Base class for all reactions.
    /// </summary>
    public abstract class ReactionBase : MonoBehaviour
    {
        /// <summary>
        /// The amount of time in seconds (real-time) before the reaction behaviour is triggered.
        /// </summary>
        [Tooltip("The amount of time in seconds (real-time) before the reaction behaviour is triggered.")]
        public float DelaySeconds = 0f;

        protected WaitForSecondsRealtime waitForSeconds;

        protected virtual void Start()
        {
            SetAndCheckReferences();
        }

        public void Init()
        {
            waitForSeconds = new WaitForSecondsRealtime(DelaySeconds);

            SpecificInit();
        }

        protected virtual void SpecificInit() {}

        public void React(MonoBehaviour monoBehaviour)
        {
            if (DelaySeconds > 0)
            {
                monoBehaviour.StartCoroutine(ReactRoutine());
            }
            else
            {
                ImmediateReaction();
            }
        }

        protected IEnumerator ReactRoutine()
        {
            yield return waitForSeconds;

            ImmediateReaction();
        }

        protected abstract void ImmediateReaction();

        protected virtual void SetAndCheckReferences()
        {
            Assert.IsTrue(DelaySeconds >= 0, $"<b>[Reaction]</b> {gameObject.name} delay seconds cannot be less than zero.");
        }
    }
}