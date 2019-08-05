using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.ReactionSystem
{
    public class ParticleEffectReaction : ReactionBase
    {
        public GameObject Prefab;
        public Transform SpawnPoint = null;

        public bool ShouldDestroy = true;
        public float DestroyTime = 2f;

        protected override void ImmediateReaction()
        {
            if (Prefab == null) return;

            var particleEffect = SpawnPoint == null ? Instantiate(Prefab) : Instantiate(Prefab, SpawnPoint.position, SpawnPoint.rotation);
            
            if(ShouldDestroy)
            {
                Destroy(particleEffect.gameObject, DestroyTime);
            }
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();

            Assert.IsNotNull(Prefab, $"<b>[ParticleEffectReaction]</b> {gameObject.name} prefab has not been assigned.");
            Assert.IsTrue(DestroyTime >= 0f, $"<b>[ParticleEffectReaction]</b> {gameObject.name} destroy time cannot be less than 0.");
        }
    }
}