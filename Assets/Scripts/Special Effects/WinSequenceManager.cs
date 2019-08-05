using UnityEngine;

public class WinSequenceManager : MonoBehaviour
{
    [SerializeField] private EndgameManager endgameManager;
    [SerializeField] private Transform confettiExplosion;
    [SerializeField] private Transform planetBalls;
    [SerializeField] private int timeBeforeTriggeringConfetti = 1;
    [SerializeField] private int confettiDuration = 5;
    [SerializeField] private int timeToPlayBeforeAlarm = 15;
    [SerializeField] private GameObject teleportWholeRoom;
    private AudioSource audioSource;
    private bool hasTriggered;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerWinSequence()
    {
        if (!hasTriggered)
        {
            teleportWholeRoom.SetActive(true);
            Invoke("StartWinSequence", timeBeforeTriggeringConfetti);
            hasTriggered = true;
        }
    }

    private void StartWinSequence()
    {
        audioSource.Play();
        confettiExplosion.gameObject.SetActive(true);
        planetBalls.gameObject.SetActive(true);
        Invoke("StopEmittingConfetti", confettiDuration);
        endgameManager.Invoke("StartSoundingAlarm", timeToPlayBeforeAlarm);
    }

    private void StopEmittingConfetti()
    {
        foreach (Transform child in confettiExplosion)
        {
            child.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
