using UnityEngine;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.ReactionSystem
{
    public class FracturableCard : MonoBehaviour
    {
        [Header("Reactions")]
        [SerializeField] private ReactionCollection reactions;

        Interactable _interactable;

        void Start()
        {
            _interactable = GetComponent<Interactable>();
            SubscribeToInteractionEvents();
        }
        
        /// <summary>
        /// Subscribes to the <see cref="Hand"/> onAttachedToHand and onDetachedFromHand events.
        /// </summary>
        private void SubscribeToInteractionEvents()
        {
            _interactable.onAttachedToHand += HandleAttachedToHand;
            _interactable.onDetachedFromHand += HandleDetachedFromHand;
        }

        private void HandleAttachedToHand(Hand hand)
        {
            if(reactions != null)
            {
                reactions.React(true);
            }
        }

        protected virtual void HandleDetachedFromHand(Hand hand) { }
        
        
        /// <summary>
        /// From <see cref="Valve.VR.InteractionSystem.Sample.InteractableExample"/>
        /// </summary>
        /// <param name="hand">The <see cref="Hand"/> hovering over this game object.</param>
        protected virtual void HandHoverUpdate(Hand hand)
        {
            var startingGrabType = hand.GetGrabStarting();

            var isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (_interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                AttachToHand(hand, startingGrabType);
            }
            else if (isGrabEnding)
            {
                DetachFromHand(hand);
            }
        }

        protected virtual void AttachToHand(Hand hand, GrabTypes startingGrabType)
        {
            hand.HoverLock(_interactable);
            hand.AttachObject(gameObject, startingGrabType);
        }

        protected virtual void DetachFromHand(Hand hand)
        {
            if (hand == null) return;

            hand.DetachObject(gameObject);
            hand.HoverUnlock(_interactable);
        }
    }
}