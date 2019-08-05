using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    [SerializeField] private FracturedChunk head;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            head.IsSupportChunk = false;
            head.GetComponent<Collider>().enabled = true;
            head.DetachFromObject();
        }
        
    }
}
