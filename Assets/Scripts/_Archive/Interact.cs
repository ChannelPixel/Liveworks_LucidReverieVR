using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

namespace ARCHIVE
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(FixedJoint))]
    public class Interact : MonoBehaviour
    {
        public SteamVR_Action_Boolean m_grabAction = null;

        private SteamVR_Behaviour_Pose m_Pose = null;
        private FixedJoint m_Joint = null;

        [Header("Interactables")] [Tooltip("The current object being held by the hand.")]
        public Interactable m_CurrentInteractable = null;

        [Tooltip("The previous object being held by the hand.")]
        public Interactable m_PreviousInteractable = null;

        private List<Interactable> m_ContactInteractables = new List<Interactable>();

        [Header("Component Values")] [Tooltip("The force that will break the fixed joint.")]
        private float baseBreakForce = 800;

        [Tooltip("The torque that will break the fixed joint.")]
        private float baseBreakTorque = 800;

        [Tooltip("The radius of the collider for grabbing objects.")]
        public float colliderRadius = 0.1f;

        private void Awake()
        {
            m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
            m_Joint = GetComponent<FixedJoint>();
            baseBreakForce = m_Joint.breakForce;
            baseBreakTorque = m_Joint.breakTorque;
        }

        // Update is called once per frame
        private void Update()
        {
            CheckComponents();

            // Down
            if (m_grabAction.GetStateDown(m_Pose.inputSource))
            {
                print(m_Pose.inputSource + " Trigger Down");
                PickUp();
            }

            // Up
            if (m_grabAction.GetStateUp(m_Pose.inputSource))
            {
                print(m_Pose.inputSource + " Trigger Up");
                Drop();
            }

            if (gameObject.GetComponent<FixedJoint>() == null)
            {
                gameObject.AddComponent<FixedJoint>();
                m_Joint = GetComponent<FixedJoint>();
                m_Joint.breakForce = baseBreakForce;
                m_Joint.breakTorque = baseBreakTorque;
            }
        }

        private void CheckComponents()
        {
            Assert.AreEqual(gameObject.GetComponent<Rigidbody>().useGravity, false,
                "The hands should not be using gravity!");
            Assert.AreEqual(gameObject.GetComponent<Rigidbody>().isKinematic, true, "The hands should be kinematic!");

            Assert.AreEqual(gameObject.GetComponent<SphereCollider>().isTrigger, true,
                "The sphere collider should be a trigger!");
            Assert.IsTrue(gameObject.GetComponent<SphereCollider>().radius < 1f,
                "The sphere collider radius should be less than 1. Try 0.1.");
        }

        private void OnJointBreak(float breakForce)
        {
            DetachInteractable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactable"))
                return;

            m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Interactable"))
                return;

            m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
        }

        public void PickUp()
        {
            // Get nearest
            m_CurrentInteractable = GetNearestInteractable();
            // Null check
            if (!m_CurrentInteractable)
            {
                return;
            }

            // Already held check
            if (m_CurrentInteractable.m_ActiveHand)
            {
                m_CurrentInteractable.m_ActiveHand.Drop();
            }

            // Position to controller
            m_CurrentInteractable.transform.position = transform.position;
            // Attach to rigidbody
            Rigidbody target = m_CurrentInteractable.GetComponent<Rigidbody>();
            m_Joint.connectedBody = target;
            // Set activehand
            m_CurrentInteractable.m_ActiveHand = this;
        }

        public void Drop()
        {
            // Null check
            if (!m_CurrentInteractable)
            {
                return;
            }

            // Apply velocity
            Rigidbody target = m_CurrentInteractable.GetComponent<Rigidbody>();
            target.velocity = m_Pose.GetVelocity();
            target.angularVelocity = m_Pose.GetAngularVelocity();

            DetachInteractable();
        }

        private void DetachInteractable()
        {
            // Detach
            if (m_Joint.connectedBody != null)
            {
                m_Joint.connectedBody = null;
            }

            // Clear
            m_CurrentInteractable.m_ActiveHand = null;
            m_PreviousInteractable = m_CurrentInteractable;
            m_CurrentInteractable = null;
        }

        private Interactable GetNearestInteractable()
        {
            Interactable nearest = null;
            float minDistance = float.MaxValue;
            float distance = 0f;

            foreach (Interactable interactable in m_ContactInteractables)
            {
                distance = (interactable.transform.position - transform.position).sqrMagnitude;

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = interactable;
                }
            }

            return nearest;
        }
    }
}