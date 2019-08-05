using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Assertions;
using ATT.Interaction.ReactionSystem;

namespace ATT.Interaction.PuzzleSystem
{
    [SelectionBase]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PuzzleObjectBase : MonoBehaviour
    {
        [Header("Puzzles")]
        public PuzzleManager PuzzleManager = null;
        [SerializeField] protected PuzzleDefinition puzzleDefinition = null;
        protected bool isComplete = false;
               
        [Header("Interaction")]
        protected bool isInteractable = true;
        public virtual bool IsInteractable => isInteractable;
        
        [Header("Reactions")]
        [SerializeField] protected ReactionCollection successReactions;
        
        public bool IsComplete => isComplete;
        public int PuzzleID => puzzleDefinition.ID;
        public virtual Puzzle Puzzle => PuzzleManager.Puzzles.Find(p => p.ID == PuzzleID);
        
        #region References
        protected Collider _collider;
        protected Interactable _interactable;
        protected Rigidbody _rigidbody;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            SetAndCheckReferences();
            SubscribeToInteractionEvents();
        }
        #endregion

        #region Puzzle Logic
        /// <summary>
        /// Invokes the SetObjectiveComplete method for the Puzzle instance.
        /// </summary>
        protected virtual void SetPuzzleObjectiveComplete()
        {
            Assert.IsTrue(Puzzle.Status == PuzzleStatus.Active);
            Puzzle.SetObjectiveComplete();

            if (Puzzle.Status == PuzzleStatus.Complete)
            {
                isComplete = true;
                successReactions.React();
            }
        }
        #endregion

        #region Hand Events

        /// <summary>
        /// Subscribes to the <see cref="Hand"/> onAttachedToHand and onDetachedFromHand events.
        /// </summary>
        protected virtual void SubscribeToInteractionEvents()
        {
            _interactable.onAttachedToHand += HandleAttachedToHand;
            _interactable.onDetachedFromHand += HandleDetachedFromHand;
        }

        protected virtual void HandleHandHoverBegin()
        {
            if (!isInteractable) return;
        }

        protected virtual void HandleHandHoverEnd()
        {
            if (!isInteractable) return;
        }

        protected virtual void HandleAttachedToHand(Hand hand) { }

        protected virtual void HandleDetachedFromHand(Hand hand) { }
        #endregion

        #region Interaction Logic
        /// <summary>
        /// From <see cref="Valve.VR.InteractionSystem.Sample.InteractableExample"/>
        /// </summary>
        /// <param name="hand">The <see cref="Hand"/> hovering over this game object.</param>
        protected virtual void HandHoverUpdate(Hand hand)
        {
            if (!isInteractable) return;

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
            if (!isInteractable) return;
            
            hand.HoverLock(_interactable);
            hand.AttachObject(gameObject, startingGrabType);
        }

        protected virtual void DetachFromHand(Hand hand)
        {
            if (hand == null) return;

            hand.DetachObject(gameObject);
            hand.HoverUnlock(_interactable);
        }
        
        protected void DisableInteraction()
        {
            _interactable.highlightOnHover = false;
            _interactable.enabled = false;
            isInteractable = false;
        }
        #endregion

        #region Debugging
        protected virtual void SetAndCheckReferences()
        {
            if(PuzzleManager == null)
            {
                PuzzleManager = FindObjectOfType<PuzzleManager>();
            }
            Assert.IsNotNull(PuzzleManager, $"<b>[PuzzleObjectBase {gameObject.name}]</b> Puzzle manager is not assigned and cannot be found in the scene.");
            
            _collider = GetComponent<Collider>();
            Assert.IsNotNull(_collider, $"<b>[PuzzleObjectBase {gameObject.name}]</b> has no Collider component.");

            _interactable = GetComponent<Interactable>();
            Assert.IsNotNull(_interactable, $"<b>[PuzzleObjectBase {gameObject.name}]</b> has no Interactable component (Valve.VR.InteractionSystem.Interactable).");

            _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_rigidbody, $"<b>[PuzzleObjectBase {gameObject.name}]</b> has no Rigidbody component.");

            Assert.IsNotNull(puzzleDefinition, $"<b>[PuzzleObjectBase {gameObject.name}]</b> has no Puzzle Definition assigned.");
            Assert.IsNotNull(successReactions, $"<b>[PuzzleObjectBase {gameObject.name}]</b> has no Default Reaction Collection assigned.");
        }
        #endregion
    }
}