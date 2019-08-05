using System;
using UnityEngine;

namespace ATT.SpecialEffects
{
    public class ph_GrowPlayer : MonoBehaviour
    {
        [SerializeField] private Transform growMagicSmokePrefab;
        [SerializeField] private Transform[] hands;
        [SerializeField] private Vector3 scaleStart = new Vector3(0.05f, 0.05f, 0.05f);
        [SerializeField] private Vector3 scaleEnd = new Vector3(1f, 1f, 1f);
        [SerializeField] private float growSpeed = 0.1f;
        [SerializeField] private bool isGrowing = false;
        [SerializeField] private bool hasGrown = false;

        private Transform smokeInstance;

        private Vector3 scaleEndAdjusted;
        private float timer;
        private bool smokeHasSpawned;

        public event Action OnGrowBehaviourComplete;

        public bool CanGrow => !isGrowing && !hasGrown;

        void Start()
        {
            scaleEndAdjusted = new Vector3(scaleEnd.x + 0.1f, scaleEnd.y + 0.1f, scaleEnd.z + 0.1f);
            transform.localScale = scaleStart;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isGrowing = true;
            }

            if (isGrowing)
            {
                GrowPlayer();
            }
        }

        public void TriggerGrowBehaviour()
        {
            if (CanGrow)
            {
                isGrowing = true;
            }
        }

        private void GrowPlayer()
        {
            if (!smokeHasSpawned)
            {
                smokeInstance = Instantiate(growMagicSmokePrefab, transform.position, growMagicSmokePrefab.rotation,
                    transform);
                smokeHasSpawned = true;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, scaleEndAdjusted, growSpeed * Time.deltaTime);

            if (transform.localScale.x > 1)
            {
                transform.localScale = Vector3.one;
                isGrowing = false;
                hasGrown = true;
                OnGrowBehaviourComplete?.Invoke();
            }
        }
    }
}