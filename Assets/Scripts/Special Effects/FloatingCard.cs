using UnityEngine;
using UnityEngine.Audio;

public class FloatingCard : MonoBehaviour
{
    [SerializeField] private Transform cardPack;
    [SerializeField] private AudioClip[] whooshAudioClips;
    [SerializeField] private int rotationSpeedMin = 60;
    [SerializeField] private int rotationSpeedMax = 180;
    [SerializeField] private float floatSpeedMin = 0.3f;
    [SerializeField] private float floatSpeedMax = 1f;
    [SerializeField] private int floatHeightMin = 5;
    [SerializeField] private int floatHeightMax = 10;
    [SerializeField] private float flySpeedMin = 7f;
    [SerializeField] private float flySpeedMax = 10f;
    private AudioSource audioSource;
    public int whooshSoundChoice;
    private float floatSpeed;
    private int floatHeight;
    private int rotationSpeed;
    private float flySpeed;
    private bool isFloatingAndRotating;
    private bool isFlyingToCardPack;
    private Vector3 pos;
    private Vector3 posNew;
    private float flyDelay;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pos = transform.position;
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        floatSpeed = Random.Range(floatSpeedMin, floatSpeedMax);
        floatHeight = Random.Range(floatHeightMin, floatHeightMax);
        flySpeed = Random.Range(flySpeedMin, flySpeedMax);
        whooshSoundChoice = Random.Range(0, whooshAudioClips.Length);
        flyDelay = Random.Range(0f, 5f);
    }

    public void StartFloatAndRot()
    {
        isFloatingAndRotating = true;
    }

    public void StartFlyToCardPack()
    {
        Invoke("StartFly", flyDelay);
    }

    private void StartFly()
    {
        isFloatingAndRotating = false;
        isFlyingToCardPack = true;
        audioSource.PlayOneShot(whooshAudioClips[whooshSoundChoice]);
    }

    private void Update()
    {
        if (isFloatingAndRotating)
        {
            Float();
            Rotate();
        }

        if (isFlyingToCardPack)
        {
            FlyToCardPack();
        }
    }

    private void FlyToCardPack()
    {
        transform.LookAt(cardPack);
        transform.position = Vector3.MoveTowards(transform.position, cardPack.position, Time.deltaTime * flySpeed);

        if (Vector3.Distance(transform.position, cardPack.position) < 0.01f)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            Destroy(gameObject, 3);
        }
    }

    private void Float()
    {
        posNew = pos;
        posNew.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatSpeed) * floatHeight / 100;

        transform.position = posNew;
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotationSpeed, 0f), Space.World);
    }


}
