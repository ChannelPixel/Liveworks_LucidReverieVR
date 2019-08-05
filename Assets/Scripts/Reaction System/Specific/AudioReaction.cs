using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.Interaction.ReactionSystem
{
    public class AudioReaction : ReactionBase
    {
        public AudioSource AudioSource;
        public AudioClip AudioClip;
        [Range(0f, 1f)] public float Volume = 0.5f;
        [Range(-1f, 2f)] public float Pitch = 1.0f;

        protected override void ImmediateReaction()
        {
            AudioSource.clip = AudioClip;
            AudioSource.volume = Volume;
            AudioSource.Play();
        }

        protected override void SetAndCheckReferences()
        {
            base.SetAndCheckReferences();
            Assert.IsNotNull(AudioSource, $"<b>[AudioReaction]</b> {gameObject.name} has no Audio Source assigned.");
            Assert.IsNotNull(AudioClip, $"<b>[AudioReaction]</b> {gameObject.name} has no Audio Clip assigned.");
        }
    }
}