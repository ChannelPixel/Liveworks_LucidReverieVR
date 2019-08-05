using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATT;
using Valve.VR;

public class EndgameManager : MonoBehaviour
{
    [SerializeField] private SteamVR_Fade steamVRFade;
    [SerializeField] private Camera vrCamera;
    [SerializeField] private AudioClip crackingSound;
    [SerializeField] private LayerMask textLayer;
    [SerializeField] private LowerBackgroundMusic radio;
    [SerializeField] private AlarmLamp alarmLamp;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject teleportWholeRoom;
    [SerializeField] private float alarmDuration = 15;
    [SerializeField] private float timeToExplode = 6;
    [SerializeField] private GameObject text;
    [SerializeField] private Transform crackingWalls;
    [SerializeField] private Material[] wallCrackMaterials;
    private ExplodeRoom explodeRoom;
    private int crackLevel;
    public bool AlarmHasStarted;

    private void Start()
    {
        explodeRoom = GetComponent<ExplodeRoom>();
    }
    public void StartSoundingAlarm()
    {
        AlarmHasStarted = true;
        radio.LowerVolume();
        teleportWholeRoom.SetActive(true);
        gameManager.StopPlayerVitalsAudio();
        Invoke("StartCrackingWall", 13);
        Invoke("IncreaseCracking", 15);
        Invoke("IncreaseCracking", 17);
        Invoke("IncreaseCracking", 18);
        Invoke("IncreaseCracking", 21);
        alarmLamp.StartAlarm();
        alarmLamp.Invoke("StartLoweringPitch", alarmDuration);
        alarmLamp.Invoke("StartShortCircuitingLight", alarmDuration);
        explodeRoom.Invoke("TriggerExplosion", alarmDuration + timeToExplode);
        Invoke("RemoveCrackingWall", alarmDuration + timeToExplode);
        Invoke("Fade", alarmDuration + timeToExplode + 34);
    }

    private void StartCrackingWall()
    {
        crackingWalls.gameObject.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(crackingSound);
    }

    private void IncreaseCracking()
    {
        foreach (Transform wall in crackingWalls)
        {
            wall.GetComponent<MeshRenderer>().material = wallCrackMaterials[crackLevel];
        }

        crackLevel++;
    }

    private void RemoveCrackingWall()
    {
        crackingWalls.gameObject.SetActive(false);
    }

    private void Fade()
    {
        steamVRFade.OnStartFade(Color.black, 2, false);
        Invoke("ShowText", 2);
    }

    private void ShowText()
    {
        vrCamera.clearFlags = CameraClearFlags.Depth;
        vrCamera.cullingMask = textLayer;
        text.SetActive(true);
        steamVRFade.OnStartFade(Color.clear, 2, false);
        Invoke("HideText", 10);
    }

    private void HideText()
    {
        steamVRFade.OnStartFade(Color.black, 2, false);
    }

    private void GoDark()
    {
        vrCamera.clearFlags = CameraClearFlags.Depth;
        vrCamera.cullingMask = textLayer;
    }
}
