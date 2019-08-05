using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.PuzzleSystem
{
    [System.Serializable]
    public class Puzzle
    {
        #region Properties
        /// <summary> The <see cref="ScriptableObject"/> required to instantiate a <see cref="Puzzle"/> object. </summary>
        private PuzzleDefinition _definition;
        
        /// <summary> The current status of the puzzle. </summary>
        [SerializeField]
        private PuzzleStatus _status;
        #endregion

        /// <summary> The number of puzzle objectives that have been met. </summary>
        [SerializeField]
        private int _completedObjectives;
        
        #region Getters
        public PuzzleDefinition Definition => _definition;
        public int ID => _definition.ID;
        public string Name => _definition.Name;
        public int RequiredObjectives => _definition.ObjectiveCount;
        public int CompletedObjectives => _completedObjectives;
        public PuzzleStatus Status => _status;
        #endregion

        #region Events
        public event Action<Puzzle> OnPuzzleStatusChanged;
        #endregion
        
        public bool ObjectivesMet() => CompletedObjectives == RequiredObjectives;

        #region Constructors
        public Puzzle(PuzzleDefinition definition)
        {
            _definition = definition;
            _status = PuzzleStatus.Inactive;
            _completedObjectives = 0;

#if UNITY_EDITOR
            SetSerialisedForDebugging(definition);
#endif
        }
        #endregion

        public void SetPuzzleActive()
        {
            #region Assertions
            Assert.IsTrue(Status == PuzzleStatus.Inactive, $"{Name} puzzle status is {Status}. Cannot set active.");
            #endregion

            _status = PuzzleStatus.Active;
            TriggerStatusChanged();
        }

        public void SetObjectiveComplete()
        {
            #region Assertions
            Assert.IsFalse(Status == PuzzleStatus.Inactive || Status == PuzzleStatus.Complete, $"Puzzle {Name} status is {Status}, cannot set objective complete.");
            Assert.IsFalse(ObjectivesMet(), $"Puzzle {Name}'s objectives have already been met.");
            #endregion
            //Debug.Log($"Puzzle {Name} incrementing objective complete count");
            _completedObjectives++;
            //Debug.Log($"{Name} completed objectives incremented.");
            if (ObjectivesMet() && Status == PuzzleStatus.Active)
            {
                _status = PuzzleStatus.Complete;
                //Debug.Log($"{Name} status changed to {_status}");
            }
            TriggerStatusChanged();
        }

        /// <summary> Invokes the <see cref="OnPuzzleStatusChanged"/> event. </summary>
        private void TriggerStatusChanged()
        {
            OnPuzzleStatusChanged?.Invoke(this);
        }

        #region Debugging
        [SerializeField, Tooltip("The PuzzleDefinition's unique instance ID")]
        private int _id;
        [SerializeField]
        private string _name;
        [SerializeField]
        private int _requiredObjectives;
        

        /// <summary> Allows serialisation of properties in the inspector. </summary>
        /// <param name="definition"></param>
        private void SetSerialisedForDebugging(PuzzleDefinition definition)
        {
            _id = definition.ID;
            _name = definition.Name;
            _requiredObjectives = definition.ObjectiveCount;
        }
        #endregion
    }
}