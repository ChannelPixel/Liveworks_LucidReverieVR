using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.ReactionSystem
{
    public class DestroyGameObjectReaction : ReactionBase
    {
        public GameObject DestroyableObject = null;
        
        protected override void ImmediateReaction()
        {
            Destroy(DestroyableObject, DelaySeconds);
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(DestroyableObject, $"<b>[DestroyGameObjectReaction]</b> {gameObject.name} has no Destroyable Object assigned.");
        }
    }
}