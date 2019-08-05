using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ATT.SpecialEffects
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerVitals : MonoBehaviour
    {
        public AudioClip HeartbeatClip = null;

        [Header("Heartbeat Volume")]
        [SerializeField] private float minVolume = 0.5f;
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private float volumeToMaxTime = 30f;
        [SerializeField] private float currentVolumeTime = 0f;

        [Header("Heartbeat Pitch")]
        [SerializeField] private float minPitch = .5f;
        [SerializeField] private float maxPitch = 1.4f;
        [SerializeField] private float pitchToMaxTime = 30f;
        [SerializeField] private float currentPitchTime = 0f;
         
        private bool shouldPlayAudio = false;
        
        [Header("Debugging")]
        public bool PlayOnAwake = false;
        public bool Loop = true;

        private Coroutine increasingVitalsRoutine;

        AudioSource _audioSource;

        void Start()
        {
            SetAndCheckReferences();
            PrepareAudioSource();
        }

        private void PrepareAudioSource()
        {
            _audioSource.playOnAwake = PlayOnAwake;
            _audioSource.loop = Loop;
            _audioSource.volume = minVolume;
            _audioSource.pitch = minPitch;
        }

        public void StartHeartbeat()
        {
            shouldPlayAudio = true;
            increasingVitalsRoutine = StartCoroutine(IncreasingVitals());
        }

        public void StopHeartbeat()
        {
            if (increasingVitalsRoutine != null)
            {
                shouldPlayAudio = false;
                _audioSource.Stop();
                StopCoroutine(increasingVitalsRoutine);
            }
        }

        IEnumerator IncreasingVitals()
        {
            _audioSource.Play();

            while (shouldPlayAudio)
            {
                currentVolumeTime += Time.deltaTime;
                _audioSource.volume = Mathf.Lerp(minVolume, maxVolume, currentVolumeTime / volumeToMaxTime);

                currentPitchTime += Time.deltaTime;
                _audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, currentPitchTime / pitchToMaxTime);
                
                yield return null;
            }

            yield break;
        }

        private void SetAndCheckReferences()
        {
            _audioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(_audioSource, $"<b>[PlayerVitals]</b> has no Audio Source component.");

            Assert.IsNotNull(HeartbeatClip, $"<b>[PlayerVitals]</b> Heartbeat audio clip has not been assigned.");
        }
    }
}