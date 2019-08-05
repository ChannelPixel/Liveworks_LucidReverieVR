using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LowerBackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private bool loweringHasStarted;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (loweringHasStarted)
        {
            LowerVolume();
        }
    }
    public void LowerVolume()
    {
        audioSource.volume = 0;
    }
}
