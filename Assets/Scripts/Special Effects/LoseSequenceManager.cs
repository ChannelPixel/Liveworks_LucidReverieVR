using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseSequenceManager : MonoBehaviour
{
    [SerializeField] private EndgameManager endgameManager;
    [SerializeField] private int timeBeforeTriggeringAlarm = 1;
    private bool hasTriggered;

    public void TriggerLoseSequence()
    {
        if (!hasTriggered)
        {
            endgameManager.Invoke("StartSoundingAlarm", timeBeforeTriggeringAlarm);
            hasTriggered = true;
        }
    }
}
