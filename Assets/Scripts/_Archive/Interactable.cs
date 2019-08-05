using UnityEngine;

namespace ARCHIVE
{
    [RequireComponent(typeof(Rigidbody))]
    public class Interactable : MonoBehaviour
    {
        [HideInInspector] public Interact m_ActiveHand = null;
    }
}