using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.ReactionSystem
{
    /// <summary>
    /// Stores a reference to all <see cref="ReactionBase"/> scripts on the game object and triggers them when the React() method is invoked.
    /// </summary>
    public class ReactionCollection : MonoBehaviour
    {
        private ReactionBase[] reactions;
        
        private bool canReact = true;

        void Awake()
        {
            SetAndCheckReferences();
        }

        void Start()
        {
            reactions = GetComponentsInChildren<ReactionBase>();

            reactions.ForEach(r => r.Init());
        }

        public void React(bool disableAfterReaction = true)
        {
            if (!canReact) return;

            reactions.ForEach(r => r.React(this));

            if (disableAfterReaction)
            {
                canReact = false;
            }
        }

        public void DisableReactionCollectionReactions()
        {
            reactions.ForEach(r => { r.enabled = false; });
        }

        private void SetAndCheckReferences()
        {
            reactions = GetComponents<ReactionBase>();
            
            Assert.IsTrue(reactions != null && reactions.Length > 0, $"<b>[ReactionCollection]</b> {gameObject.name} has no reactions assigned.");
        }
    }
}