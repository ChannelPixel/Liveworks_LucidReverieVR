using System;
using UnityEngine;

namespace ATT.SpecialEffects
{
    public class ph_ExplodeRoom : MonoBehaviour
    {
        [SerializeField] private FracturedObject[] walls;
        [SerializeField] private float explosionForce;
        [SerializeField] private Transform furniture;

        [SerializeField] private bool hasExploded = false;

        public event Action OnExplodeRoom;

        public bool CanExplode => !hasExploded;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                ExplodeRoom();
            }
        }

        public void TriggerExplodeRoom()
        {
            if (!hasExploded)
            {
                ExplodeRoom();
            }
        }

        private void ExplodeRoom()
        {
            hasExploded = true;
            OnExplodeRoom?.Invoke();

            Physics.gravity = Vector3.zero;

            foreach (FracturedObject wall in walls)
            {
                wall.Explode(Vector3.zero, explosionForce);
            }

            foreach (Transform child in furniture)
            {
                if (child.GetComponent<Rigidbody>() != null)
                {
                    child.GetComponent<Rigidbody>().AddExplosionForce(50, Vector3.zero, 10, 0, ForceMode.Impulse);
                }
            }
        }
    }
}