using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction.ReactionSystem
{
    /// <summary>
    /// NOTE: If you get an error telling you you can't destroy assets, the reference to the Interactable is probably a prefab. It needs to be an instance.
    /// </summary>
    public class DisableInteractionReaction : ReactionBase
    {
        public Interactable Interactable;
        
        protected override void ImmediateReaction()
        {
            var go = Interactable.gameObject;
            
            if (go.GetComponent<PhysicsMaterialController>() != null)
            {
                Destroy(go.GetComponent<PhysicsMaterialController>());
            }

            if (go.GetComponent<Throwable>() != null)
            {
                Destroy(go.GetComponent<Throwable>());
            }

            if (go.GetComponent<VelocityEstimator>() != null)
            {
                Destroy(go.GetComponent<VelocityEstimator>());
            }

            if (go.GetComponent<ReturnToOrigin>() != null)
            {
                Destroy(go.GetComponent<ReturnToOrigin>());
            }

            Destroy(Interactable);
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(Interactable, $"<b>[DisableInteractionReaction]</b> {gameObject.name} Interactable has not been assigned.");
        }
    }
}