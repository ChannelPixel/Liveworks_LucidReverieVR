using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.ReactionSystem
{
    public class AnimationReaction : ReactionBase
    {
        public Animator Animator;
        public string Trigger;

        private int triggerHash;
        
        protected override void SpecificInit()
        {
            triggerHash = Animator.StringToHash(Trigger);
        }

        protected override void ImmediateReaction()
        {
            Animator.SetTrigger(triggerHash);
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(Animator, $"<b>[AnimationReaction]</b> {gameObject.name} Animator component has not been assigned.");
            Assert.IsFalse(Trigger == string.Empty, $"<b>[AnimationReaction]</b> {gameObject.name} trigger value for animation is an empty string.");
        }
    }
}