using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.ReactionSystem
{
    public class FractureInteractableReaction : ReactionBase
    {
        public Interactable FracturableInteractable;
        public FracturedChunk head;
        public Transform rightHand;
        public Transform leftHand;
        [SerializeField] private EndgameManager endgameManager;
        [SerializeField] private Transform clutter;
        [SerializeField] private FloatingCardManager floatingCardManager;
        [SerializeField] private LayerMask layerPlayerBody;
        [SerializeField] private Transform queenHeartsReactionCollection;
        [SerializeField] private AudioClip offWithTheirHeadsClip;
        [SerializeField] private float decapitationDelay;
        private bool isCheckingHandRotation;
        public bool hasDecapitated;
        private RaycastHit hit;
        private AudioSource audioSource;
        private AudioSource queenHeartsAudioSource;
        private FractureInteractableReaction queenHeartsFractureInteractableReaction;

        private void Awake()
        {
            audioSource = FracturableInteractable.GetComponent<AudioSource>();
            queenHeartsAudioSource = queenHeartsReactionCollection.parent.GetComponent<AudioSource>();
            queenHeartsFractureInteractableReaction = queenHeartsReactionCollection.GetComponent<FractureInteractableReaction>();
        }
        private void Update()
        {
            if (isCheckingHandRotation && !hasDecapitated && !queenHeartsFractureInteractableReaction.hasDecapitated && !endgameManager.AlarmHasStarted)
            {
                CheckHandRotation();
            }
        }

        protected override void ImmediateReaction()
        {
            if (FracturableInteractable == null || FracturableInteractable.attachedToHand == null) return;
            isCheckingHandRotation = true;
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(FracturableInteractable, $"<b>[FractureInteractableReaction]</b> {gameObject.name} Fracturable Interactable has not been assigned.");
        }

        private void CheckHandRotation()
        {
            if (FracturableInteractable.transform.parent == rightHand || FracturableInteractable.transform.parent == leftHand)
            {
                if (Physics.Raycast(transform.position, -FracturableInteractable.transform.forward, out hit, 100, layerPlayerBody))
                {
                    queenHeartsAudioSource.PlayOneShot(offWithTheirHeadsClip);
                    Invoke("TriggerDecapitation", decapitationDelay);
                    isCheckingHandRotation = false;
                    hasDecapitated = true;
                }

            }
        }

        private void TriggerDecapitation()
        {
            audioSource.Play();
            head.IsSupportChunk = false;
            head.GetComponent<Collider>().enabled = true;
            head.DetachFromObject();
            head.transform.parent = clutter;

            if (transform.parent.gameObject.name == "Card_QueenHearts_Fractured")
            {
                floatingCardManager.FloatAndRotateCards();
            }
        }
    }
}
