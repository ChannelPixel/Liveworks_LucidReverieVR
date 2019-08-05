using UnityEngine;

namespace ATT.Interaction.PuzzleSystem
{
    [CreateAssetMenu(menuName = "ATT/Puzzle Definition", fileName = "New Puzzle Definition")]
    public class PuzzleDefinition : ScriptableObject
    {
        /// <summary>
        /// The unique ID of the <see cref="ScriptableObject"/> .asset file.
        /// Useful for comparing references to a specific <see cref="PuzzleDefinition"/> and/or <see cref="Puzzle"/> between scripts, GameObjects, etc.
        /// </summary>
        [SerializeField, Tooltip("The unique ID of the ScriptableObject .asset file. NOTE: this is self generated. If you change it, it will change back.")]
        protected int id;
        public int ID => id;

        /// <summary> The name assigned to the puzzle. Probably unnecessary. </summary>
        [Tooltip("The puzzle's name.")]
        public string Name;
        
        /// <summary> The number of objectives to be met in order to complete the puzzle. </summary>
        [Tooltip("The number of objectives to be met in order to complete the puzzle.")]
        public int ObjectiveCount;
        
        #region Unity Methods
        /// <summary>
        /// Sets the unique instance ID of the <see cref="ScriptableObject"/> .asset file.
        /// </summary>
        protected virtual void OnValidate()
        {
            id = GetInstanceID();
        }
        #endregion
    }
}