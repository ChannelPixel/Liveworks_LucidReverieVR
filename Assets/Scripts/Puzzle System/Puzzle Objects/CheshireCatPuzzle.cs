using ATT.Interaction.ReactionSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.PuzzleSystem
{
    /// <summary>
    /// Logic for the Cheshire cat cushion / poster puzzle.
    /// TODO: test throwing triggers success reactions as expected. May need to adjust collision detection settings.
    /// </summary>
    [RequireComponent(typeof(ReturnToOrigin))]
    public class CheshireCatPuzzle : PuzzleObjectBase
    {
        [SerializeField]
        private ReactionCollection cushionPickupReaction = null;

        [Header("Poster")]
        [SerializeField]
        private Collider targetTrigger = null;
        
        #region Unity Methods
        void OnTriggerEnter(Collider other)
        {
            if (other == targetTrigger)
            {
                SetPuzzleObjectiveComplete();
                targetTrigger.enabled = false;
            }
        }
        #endregion

        #region Hand Events
        protected override void HandleAttachedToHand(Hand hand)
        {
            cushionPickupReaction.React(false);
        }
        #endregion
        
        #region Debugging
        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();

            Assert.IsNotNull(targetTrigger, $"<b>{gameObject.name}'s</b> receiver trigger (collider) has not been assigned.");
            Assert.IsTrue(targetTrigger.isTrigger, $"{gameObject.name}'s receiver trigger (collider) isTrigger is false. Should be true.");
            targetTrigger.enabled = true;

            Assert.IsNotNull(GetComponent<ReturnToOrigin>(), $"<b>[{gameObject.name}]</b> has not ReturnToOrigin script attached.");

            Assert.IsNotNull(successReactions, $"{gameObject.name} has no success reaction collection.");
        }
        #endregion
    }
}