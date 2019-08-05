using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.PuzzleSystem
{
    public class PuzzleManager : MonoBehaviour
    {
        private const int puzzleCount = 3;
        //private const int puzzleCount = 4;

        [SerializeField] private PuzzleDefinition[] puzzleDefinitions = new PuzzleDefinition[puzzleCount];

        [Header("Debugging")]
        [SerializeField] private int completedPuzzles = 0;
        [SerializeField] private bool setActiveOnStart = false;
        [SerializeField] private List<Puzzle> puzzles = null;
       
        #region Delegates and Events
        public event Action OnAllPuzzlesCompleted;
        #endregion

        #region Getters
        public List<Puzzle> Puzzles => puzzles;
        public bool AllPuzzlesSolved => completedPuzzles == puzzleCount;
        #endregion

        #region Unity Methods

        void Awake()
        {
            SetAndCheckReferences();
        }

        void Start()
        {
            InstantiateScenePuzzles();
        }
        
        #endregion

        /// <summary>
        /// Iterates over <see cref="puzzleDefinitions"/> and instantiates each puzzle, subscribes to its status change event, 
        /// adds the puzzle to a list and sets the puzzle active if <see cref="setActiveOnStart"/> is true.
        /// </summary>
        private void InstantiateScenePuzzles()
        {
            puzzles = new List<Puzzle>(puzzleCount);
            puzzleDefinitions
                .ToList()
                .ForEach(d =>
                {
                    var puzzle = new Puzzle(d);
                    puzzle.OnPuzzleStatusChanged += HandlePuzzleStatusChanged;
                    puzzles.Add(puzzle);
                    puzzle.SetPuzzleActive();
                });
        }
        
        /// <summary>
        /// Invoked when the status of a <see cref="Puzzle"/> in the list changes.
        /// Increments the completed puzzle count and checks if all puzzles have been completed.
        /// If so, <see cref="OnAllPuzzlesCompleted"/> event is invoked.
        /// </summary>
        /// <param name="puzzle">Puzzle with status change.</param>
        private void HandlePuzzleStatusChanged(Puzzle puzzle)
        {
            if (puzzles.Contains(puzzle) && puzzle.Status == PuzzleStatus.Complete)
            {
                completedPuzzles++;
                if (HasCompletedAllPuzzles())
                {
                    Debug.Log($"<b>[PuzzleManager]</b> all puzzles complete.");
                    OnAllPuzzlesCompleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// Checks if all <see cref="Puzzle"/>'s (instantiated based on <see cref="PuzzleDefinition"/>'s) have been completed.
        /// </summary>
        /// <returns>True if all puzzles have been completed.</returns>
        private bool HasCompletedAllPuzzles()
        {
            var count = 0;
            puzzles.ForEach(p =>
            {
                if (p.Status == PuzzleStatus.Complete)
                {
                    count++;
                }
            });
            
            return count == completedPuzzles && count == puzzles.Count;
        }

        

        #region Debugging
        private void SetAndCheckReferences()
        {
            Assert.IsTrue(puzzleDefinitions.Length > 0, $"[PuzzleManager] No puzzle definitions have been assigned.");
            Assert.IsTrue(puzzleDefinitions.Length == puzzleCount, $"[PuzzleManager] There should be {puzzleCount} puzzle definitions assigned. There is/are {puzzleDefinitions.Length}.");
        }
        #endregion
    }
}