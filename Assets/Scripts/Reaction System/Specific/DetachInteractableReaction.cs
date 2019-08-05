using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.ReactionSystem
{
    public class DetachInteractableReaction : ReactionBase
    {
        public Interactable Interactable;

        protected override void ImmediateReaction()
        {
            if (Interactable == null || Interactable.attachedToHand == null) return;

            Interactable.attachedToHand.DetachObject(Interactable.gameObject);
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(Interactable, $"<b>[DetachInteractableReaction]</b> {gameObject.name} Interactable has not been assigned.");
        }

        public void Testing()
        {
            Interactable.attachedToHand.DetachObject(Interactable.gameObject);
        }
    }
}