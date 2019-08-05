using System.Collections;
using UnityEngine;

public class AlarmLamp : MonoBehaviour
{
    [SerializeField] private Light alarmLight;
    [SerializeField] private float alarmFlashSpeed = 5;
    [SerializeField] private float alarmFlashIntensity = 3;
    [SerializeField] private float pitchDownSpeed = 0.5f;
    [SerializeField] private float pitchFinish = 0.5f;
    [SerializeField] private float lowerVolumeSpeed = 0.5f;
    [SerializeField] private bool alarmHasStarted;
    private AudioSource audioSource;
    public bool pitchIsLowering;
    private bool lightIsFlashing;
    private bool volumeIsLowering;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void StartLoweringPitch()
    {
        audioSource.Play();
        pitchIsLowering = true;
    }

    private void Update()
    {
        if (pitchIsLowering)
        {
            LowerPitch();
        }

        if (volumeIsLowering)
        {
            LowerVolume();
        }
    }

    private void LowerPitch()
    {
        audioSource.pitch = Mathf.Lerp(audioSource.pitch, pitchFinish, pitchDownSpeed * Time.deltaTime);

        if (audioSource.pitch <= 0.3f)
        {
            pitchIsLowering = false;
            audioSource.Play();
            volumeIsLowering = true;
        }
    }

    public void StartShortCircuitingLight()
    {
        lightIsFlashing = false;
        alarmLight.intensity = alarmFlashIntensity;
        StartCoroutine(ShortCircuitLight());
    }

    private IEnumerator ShortCircuitLight()
    {
        int count = 0;

        while (count < 10)
        {
            yield return new WaitForSeconds(0.2f);
            alarmLight.enabled = !alarmLight.enabled;
            count++;
        }
    }

    public void LowerVolume()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0, lowerVolumeSpeed * Time.deltaTime);
    }

    public void StartAlarm()
    {
        StartCoroutine(SoundAlarm());
    }

    private IEnumerator SoundAlarm()
    {
        alarmHasStarted = true;
        alarmLight.enabled = true;
        lightIsFlashing = true;
        bool intensityIsIncreasing = true;
        bool timeToPlayAudio = true;

        while (alarmHasStarted)
        {
            if (timeToPlayAudio)
            {
                audioSource.Play();
                timeToPlayAudio = false;
            }

            if (intensityIsIncreasing & lightIsFlashing)
            {
                alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, alarmFlashIntensity, alarmFlashSpeed * Time.deltaTime);

                if (alarmLight.intensity > alarmFlashIntensity - 0.1f)
                {
                    intensityIsIncreasing = false;
                }
            }
            else if (!intensityIsIncreasing & lightIsFlashing)
            {
                alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0, alarmFlashSpeed * Time.deltaTime);

                if (alarmLight.intensity < 0.25f)
                {
                    intensityIsIncreasing = true;
                    timeToPlayAudio = true;
                }
            }

            yield return null;
        }
    }
}
