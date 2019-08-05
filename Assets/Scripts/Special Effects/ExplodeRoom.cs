using UnityEngine;
using ATT.Interaction;

public class ExplodeRoom : MonoBehaviour
{
    [SerializeField] private AudioClip explosion;
    [SerializeField] private float explosionLoudness = 1;
    [SerializeField] private FracturedObject[] walls;
    [SerializeField] private float explosionForceWalls = 2500f;
    [SerializeField] private float explosionForceFurniture = 100f;
    [SerializeField] private float explosionForceClutter = 5f;
    [SerializeField] private Transform furniture;
    [SerializeField] private Transform clutter;
    [SerializeField] private Transform puzzleObjects;
    [SerializeField] private Transform planetBalls;
    [SerializeField] private Transform[] lamps;
    [SerializeField] private Material skyboxSpace;
    [SerializeField] private ParticleSystem teapotSteam;
    [SerializeField] private Light spaceLight;
    [SerializeField] private Color spaceColor;
    [SerializeField] private ReturnToOrigin[] returnToOriginObjects;
    private bool isRumbling;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isRumbling)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0.3f, 0.1f * Time.deltaTime);
        }
    }

    private void StartRumble()
    {
        audioSource.Play();
        isRumbling = true;
    }

    public void TriggerExplosion()
    {
        audioSource.PlayOneShot(explosion, explosionLoudness);
        Invoke("StartRumble", 2f);
        DisablePoweredObjects();
        Physics.gravity = Vector3.zero;
        RenderSettings.skybox = skyboxSpace;
        Component[] rigidbodies = puzzleObjects.GetComponentsInChildren<Rigidbody>();
        Destroy(teapotSteam);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = spaceColor;
        spaceLight.enabled = true;
        Invoke("ChangePlanetBallDrag", 5);

        foreach (FracturedObject wall in walls)
        {
            wall.SupportChunksAreIndestructible = false;
            wall.Explode(Vector3.up, explosionForceWalls);
        }

        foreach (ReturnToOrigin obj in returnToOriginObjects)
        {
            obj.TetherDistance = 10000;
        }

        foreach (Transform child in furniture)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (rb.isKinematic)
                {
                    rb.isKinematic = false;
                }

                rb.AddExplosionForce(explosionForceFurniture, Vector3.up, 15, 0, ForceMode.Impulse);
            }
        }

        foreach (Transform child in clutter)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (rb.isKinematic)
                {
                    rb.isKinematic = false;
                }

                rb.AddExplosionForce(explosionForceClutter, Vector3.up, 10, 0, ForceMode.Impulse);
            }
        }

        foreach (Transform child in planetBalls)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.drag = 2;
                rb.angularDrag = 0.4f;
                rb.AddExplosionForce(explosionForceClutter, Vector3.down, 10, 1, ForceMode.Impulse);
            }
        }

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForceClutter, Vector3.up, 10, 0, ForceMode.Impulse);
        }
    }

    public void DisablePoweredObjects()
    {
        foreach (Transform lamp in lamps)
        {
            lamp.GetComponent<Light>().enabled = false;
            
            if (lamp.parent.GetComponent<MeshRenderer>() != null)
            {
                lamp.parent.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }
    }

    private void ChangePlanetBallDrag()
    {
        foreach (Transform child in planetBalls)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.drag = 0;
                rb.angularDrag = 0.05f;
            }
        }
    }
}