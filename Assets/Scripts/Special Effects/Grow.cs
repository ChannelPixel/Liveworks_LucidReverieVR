using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

namespace ATT.SpecialEffects
{
    public class Grow : MonoBehaviour
    {
        public GameManager GameManager = null;
        [SerializeField] private Transform growMagicSmokePrefab;
        [SerializeField] private Transform playerHead;
        [SerializeField] private Transform[] hands;
        [SerializeField] private AudioClip growPopSound;
        [SerializeField] private Vector3 scaleStart = new Vector3(0.05f, 0.05f, 0.05f);
        [SerializeField] private Vector3 scaleEnd = new Vector3(1f, 1f, 1f);
        [SerializeField] private float growSpeed = 0.1f;
        [SerializeField] private bool isGrowing;
        [SerializeField] private Transform[] teleportPoints;
        private AudioSource audioSource;
        private Transform smokeInstance;
        private Vector3 scaleEndAdjusted;
        private float timer;
        private bool smokeHasSpawned;

        void Start()
        {
            if(GameManager == null)
            {
                GameManager = FindObjectOfType<GameManager>();
            }
            Assert.IsNotNull(GameManager, $"<b>[GameManager]</b> GameManager is not assigned and cannot be found in the scene.");

            audioSource = GetComponent<AudioSource>();
            scaleEndAdjusted = new Vector3(scaleEnd.x + 0.3f, scaleEnd.y + 0.3f, scaleEnd.z + 0.3f);
            transform.localScale = scaleStart;
        }

        void Update()
        {
            if (isGrowing)
            {
                GrowPlayer();
            }
        }

        public void StartGrowingPlayer()
        {
            isGrowing = true;
            audioSource.Play();
        }

        private void GrowPlayer()
        {
            if (!smokeHasSpawned)
            {
                smokeInstance = Instantiate(growMagicSmokePrefab, transform.position, growMagicSmokePrefab.rotation, transform);
                smokeHasSpawned = true;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, scaleEndAdjusted, growSpeed * Time.deltaTime);

            if (transform.localScale.x > 1)
            {
                transform.localScale = Vector3.one;
                isGrowing = false;
                audioSource.PlayOneShot(growPopSound);
                GameManager.GrowComplete();
            }
        }
    }
}