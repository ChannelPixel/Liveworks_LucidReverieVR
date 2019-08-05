using System;
using System.Collections;
using ATT.Interaction.ReactionSystem;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.PuzzleSystem
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TeaCupReceiverPair : MonoBehaviour
    {
        [SerializeField] private Suit suit;
        [SerializeField] private Collider teaCupReceiver;
        [SerializeField] private Transform receiveTransform = null;
        [SerializeField] private ReactionCollection successReaction = null;
        [SerializeField] private float receiveTime = 1f;

        [SerializeField] private bool pairedSuccessfully = false;

        private bool inCorrectTrigger = false;

        Coroutine waitForDropRoutine;
        Coroutine receiveRoutine;

        Collider _collider;
        Interactable _interactable;
        Rigidbody _rigidbody;

        public event Action OnPairedSuccessfully;

        public Suit Suit => suit;
        public bool CanReceive => inCorrectTrigger && _interactable.attachedToHand == null && !pairedSuccessfully;


        void Start()
        {
            SetAndCheckReferences();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other == teaCupReceiver && _interactable.attachedToHand != null)
            {
                //Debug.Log($"<b>[Tea Cup {suit}]</b> entered required receiver.");
                inCorrectTrigger = true;
            }
            waitForDropRoutine = StartCoroutine(WaitForDropRoutine());
        }

        void OnTriggerExit(Collider other)
        {
            if (other == teaCupReceiver && _interactable.attachedToHand != null)
            {
                //Debug.Log($"<b>[Tea Cup {suit}]</b> exited required receiver.");
                inCorrectTrigger = false;
                if (waitForDropRoutine != null)
                {
                    StopCoroutine(waitForDropRoutine);
                }
            }
        }
        
        IEnumerator WaitForDropRoutine()
        {
            yield return new WaitUntil(() => CanReceive);

            pairedSuccessfully = true;
            OnPairedSuccessfully?.Invoke();

            receiveRoutine = StartCoroutine(ReceiveReceivable());
        }

        IEnumerator ReceiveReceivable()
        {
            successReaction.React(true);

            _rigidbody.isKinematic = true;

            var elapsedTime = 0f;
            var startPosition = transform.position;
            var startRotation = transform.rotation;

            while (elapsedTime <= receiveTime)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, receiveTransform.position, (elapsedTime / receiveTime));
                transform.rotation = Quaternion.Lerp(startRotation, receiveTransform.rotation, (elapsedTime / receiveTime));
                yield return null;
            }

            transform.position = receiveTransform.position;
            transform.rotation = receiveTransform.rotation;
        }

        private void SetAndCheckReferences()
        {
            _collider = GetComponent<Collider>();
            Assert.IsNotNull(_collider);

            _interactable = GetComponent<Interactable>();
            Assert.IsNotNull(_interactable);

            _rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(_rigidbody);

            Assert.IsNotNull(receiveTransform);

            Assert.IsNotNull(successReaction);
        }
    }
}