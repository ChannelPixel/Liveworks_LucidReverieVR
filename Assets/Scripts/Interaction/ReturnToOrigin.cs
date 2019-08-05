using UnityEngine;
using Valve.VR.InteractionSystem;

namespace ATT.Interaction
{
    public class ReturnToOrigin : MonoBehaviour
    {
        [Tooltip("The focal point for the tether. Leave as null to use this game object's initial spawn position.")]
        public GameObject TetherFocus = null;

        [Tooltip("The distance at which to return to origin.")]
        public float TetherDistance = 2f;

        public float RandomCircleRadius = 2f;

        public Throwable throwableScript = null;
        Vector3 originalPosition;
        Quaternion originalRotation;
        bool held = false;

        private void Awake()
        {
            originalPosition = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;

            if (throwableScript == null)
            {
                throwableScript = gameObject.GetComponent<Throwable>();
            }
            AddEventListeners();
        }

        private bool CheckPositionIsEmpty(Vector3 position)
        {
            return !Physics.CheckBox(position, gameObject.GetComponent<Collider>().bounds.size / 2.2f, originalRotation);
        }

        private Vector3 FindEmptyPosition()
        {
            Vector3 newPosition = originalPosition;
            bool found = false;
            int i = 0;
            while(!found)
            {
                newPosition = Random.insideUnitSphere * RandomCircleRadius + originalPosition;
                if(newPosition.y < originalPosition.y)
                {
                    newPosition.y = originalPosition.y * 1.2f;
                }
                found = CheckPositionIsEmpty(newPosition);
                Debug.Log("Found position after this many loops: " + i.ToString());
                i++;
                if(i > 100)
                {
                    Debug.Log("Couldnt find an empty position.");
                    break;
                }
            }
            return newPosition;
        }

        public void ResetPosition()
        {
            if(CheckPositionIsEmpty(originalPosition))
            {
                Debug.Log("Spawned at original position");
                gameObject.transform.position = originalPosition;
                gameObject.transform.rotation = originalRotation;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                Debug.Log("Original position not empty");
                gameObject.transform.position = FindEmptyPosition();
                gameObject.transform.rotation = originalRotation;
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("ReturnZone"))
            {
                if (!held)
                {
                    ResetPosition();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("ReturnZone"))
            {
                if (!held)
                {
                    ResetPosition();
                }
            }
        }

        private void SetHeld()
        {
            held = true;
        }

        private void SetNotHeld()
        {
            held = false;
        }

        private void AddEventListeners()
        {
            throwableScript.onPickUp.AddListener(SetHeld);
            throwableScript.onDetachFromHand.AddListener(SetNotHeld);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (!held)
            {
                if (TetherFocus == null || TetherFocus == gameObject)
                {
                    if (Vector3.Distance(gameObject.transform.position, originalPosition) > TetherDistance)
                    {
                        ResetPosition();
                    }
                }
                else
                {
                    if (Vector3.Distance(gameObject.transform.position, TetherFocus.transform.position) > TetherDistance)
                    {
                        ResetPosition();
                    }
                }
            }
        }
    }
}