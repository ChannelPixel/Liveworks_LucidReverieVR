using System;
using System.Linq;
using ATT.Interaction.ReactionSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.PuzzleSystem
{
    public class TeaSetPuzzle : MonoBehaviour
    {
        private const int teaCupCount = 4;

        public PuzzleManager PuzzleManager = null;

        [Header("Puzzle")]
        [SerializeField] private PuzzleDefinition puzzleDefinition = null;
        public int PuzzleID => puzzleDefinition.ID;
        public virtual Puzzle Puzzle => PuzzleManager.Puzzles.Find(p => p.ID == PuzzleID);

        [Header("Reactions")]
        [SerializeField] private ReactionCollection completeSetReaction;

        private int completedPairs = 0;

        TeaCupReceiverPair[] teaCupPairs = new TeaCupReceiverPair[teaCupCount];

       
        #region Unity Methods

        void Start()
        {
            SetAndCheckReferences();
        }

        #endregion


        #region Debugging

        private void SetAndCheckReferences()
        {
            if(PuzzleManager == null)
            {
                PuzzleManager = FindObjectOfType<PuzzleManager>();
            }
            Assert.IsNotNull(PuzzleManager, $"<b>[TeaSetPuzzle]</b> Puzzle Manager is not assigned and cannot be found in the scene.");

            GetTeaCupReferences();
        }

        #endregion

        private void GetTeaCupReferences()
        {
            teaCupPairs = gameObject.GetComponentsInChildren<TeaCupReceiverPair>();
            var hasClubs = false;
            var hasDiamonds = false;
            var hasHearts = false;
            var hasSpades = false;

            teaCupPairs.ToList().ForEach(t =>
            {
                t.OnPairedSuccessfully += HandlePairedSuccessfully;

                switch (t.Suit)
                {
                    case Suit.Clubs:
                        Assert.IsFalse(hasClubs, $"<b>[TeaSetPuzzle]</b> there is more than one Tea Cup with Suit set to {t.Suit}");
                        if (!hasClubs)
                        {
                            hasClubs = true;
                        }
                        break;
                    case Suit.Diamonds:
                        Assert.IsFalse(hasDiamonds, $"<b>[TeaSetPuzzle]</b> there is more than one Tea Cup with Suit set to {t.Suit}");
                        if (!hasDiamonds)
                        {
                            hasDiamonds = true;
                        }
                        break;
                    case Suit.Hearts:
                        Assert.IsFalse(hasHearts, $"<b>[TeaSetPuzzle]</b> there is more than one Tea Cup with Suit set to {t.Suit}");
                        if (!hasHearts)
                        {
                            hasHearts = true;
                        }
                        break;
                    case Suit.Spades:
                        Assert.IsFalse(hasSpades,$"<b>[TeaSetPuzzle]</b> there is more than one Tea Cup with Suit set to {t.Suit}");
                        if (!hasSpades)
                        {
                            hasSpades = true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
            Assert.IsTrue(hasClubs && hasDiamonds && hasHearts && hasSpades, $"<b>[TeaSetPuzzle]</b> did not find all suits in tea cups.");
        }

        private void HandlePairedSuccessfully()
        {
            completedPairs++;
            Puzzle.SetObjectiveComplete();
            if (Puzzle.Status == PuzzleStatus.Complete)
            {
                completeSetReaction.React(true);
            }
        }
    }
}