using System;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.PuzzleSystem
{
    /// <summary>
    /// Logic for the Chew Toy puzzle.
    /// Throwable / return scripts are destroyed if they are attached.
    /// </summary>
    public class ChewToy : PuzzleObjectBase
    {
        [Header("Mouth Collider Settings")]
        [SerializeField, Tooltip("The mouth trigger collider radius.")]
        private float radius = 0.1f;

        [SerializeField, Tooltip("The mouth trigger collider centre.")]
        private Vector3 centre = Vector3.zero;

        private bool destroyUnnecessaryComponents = true;
        
        /// <summary>
        /// Stores a reference to the HMD collider.
        /// </summary>
        private SphereCollider mouthCollider = null;

        /// <summary>
        /// Whether the Chew Toy game object is attached to a <see cref="Hand"/>.
        /// </summary>
        private bool isAttachedToHand = false;

        /// <summary>
        /// Invoked when the Chew Toy is attached to a <see cref="Hand"/> and enters the mouth collider.
        /// </summary>
        public event Action OnChewToyChewed;

        #region Unity Methods

        void Start()
        {
            AttachMouthTriggerCollider();            
        }
        
        /// <summary>
        /// Resets the HMD collider, sets the objective met flag to true and triggers the success reaction if the chew toy is attached to a <see cref="Hand"/> and enters the HMD trigger collider.
        /// </summary>
        /// <param name="other">The collider attached to other game object involved in the trigger / collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other == mouthCollider && isAttachedToHand && !IsComplete)
            {
                Destroy(mouthCollider);
                SetPuzzleObjectiveComplete();
                OnChewToyChewed?.Invoke();
            }
        }
        #endregion

        #region Hand Events
        /// <summary>
        /// Enables the mouth trigger collider.
        /// </summary>
        /// <param name="hand">The hand the game object was attached to.</param>
        protected override void HandleAttachedToHand(Hand hand)
        {
            if(!isComplete)
            {
                isAttachedToHand = true;
                mouthCollider.enabled = true;
            }
        }

        /// <summary>
        /// Disables the mouth trigger collider.
        /// </summary>
        /// <param name="hand">The <see cref="Hand"/> the game object was detached from.</param>
        protected override void HandleDetachedFromHand(Hand hand)
        {
            if(!isComplete)
            {
                isAttachedToHand = false;
                mouthCollider.enabled = false;
            }
        }
        #endregion

        #region Mouth Collider Settings
        /// <summary>
        /// Attaches a sphere collider (<see cref="mouthCollider"/>) to the game object the <see cref="Player"/>'s head collider is attached to.
        /// </summary>
        private void AttachMouthTriggerCollider()
        {
            var headCollider = Player.instance.headCollider.gameObject;
            Assert.IsNotNull(headCollider, $"<b>[ChewToy]</b> cannot find head collider on Player.");

            mouthCollider = headCollider.gameObject.AddComponent<SphereCollider>();
            mouthCollider.isTrigger = true;
            mouthCollider.center = centre;
            mouthCollider.radius = radius;
            mouthCollider.enabled = false;
        }
        #endregion

        #region Debugging
        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            
            if(destroyUnnecessaryComponents)
            {
                DestroyUnnecessaryComponents();
            }
        }

        /// <summary>
        /// Destroys <see cref="Throwable"/>, <see cref="ModalThrowable"/>, <see cref="VelocityEstimator"/> and <see cref="ComplexThrowable"/> if attached.
        /// </summary>
        private void DestroyUnnecessaryComponents()
        {
            var destroyed = false;
            if (GetComponent<Throwable>())
            {
                Destroy(GetComponent<Throwable>());
                destroyed = true;
            }

            if (GetComponent<ModalThrowable>())
            {
                Destroy(GetComponent<ModalThrowable>());
                destroyed = true;
            }

            if (GetComponent<VelocityEstimator>())
            {
                Destroy(GetComponent<VelocityEstimator>());
                destroyed = true;
            }

            if (GetComponent<ComplexThrowable>())
            {
                Destroy(GetComponent<ComplexThrowable>());
                destroyed = true;
            }

            if (GetComponent<ReturnToOrigin>())
            {
                Destroy(GetComponent<ReturnToOrigin>());
                destroyed = true;
            }

            if (destroyed)
            {
                Debug.LogWarning($"[ChewToy] at least one script attached to {gameObject.name} was destroyed.");
            }
        }
        #endregion
    }
}