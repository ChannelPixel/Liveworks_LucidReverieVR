using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class PhysicsMaterialController : MonoBehaviour
{
    [Header("Friction/Bounciness")]
    [Tooltip("Dynamic Friction to use when object is in the hand. Set to -1 to use original value.")]
    public float DynamicFriction = -1;
    [Tooltip("Static Friction to use when object is in the hand. Set to -1 to use original value.")]
    public float StaticFriction = -1;
    [Tooltip("Bounciness to use when object is in the hand. Set to -1 to use original value.")]
    public float Bounciness = 0;
    [Header("Combines")]
    [Tooltip("Whether or not the script should modify the friction combine.")]
    public bool ChangeFrictionCombine = false;
    [Tooltip("FrictionCombine to use when object is in the hand. Will only change if different to original value.")]
    public PhysicMaterialCombine FrictionCombine;
    [Tooltip("Whether or not the script should modify the friction combine.")]
    public bool ChangeBounceCombine = true;
    [Tooltip("BounceCombine to use when object is in the hand. Will only change if different to original value.")]
    public PhysicMaterialCombine BounceCombine = PhysicMaterialCombine.Minimum;

    [Header("Components")]
    [Tooltip("The material to modify. Leave null to use the one attached to this game object.")]
    public PhysicMaterial material = null;
    [Tooltip("The throwable script to monitor for value changing. Leave null to use the one attached to this game object.")]
    public Throwable throwableScript = null;

    private float OriginalDynamicFriction = 0.5f;
    private float OriginalStaticFriction = 0.5f;
    private float OriginalBounciness = 0.5f;
    private PhysicMaterialCombine OriginalFrictionCombine;
    private PhysicMaterialCombine OriginalBounceCombine;

    private float OtherColliderOriginalBounciness;

    #region Unity Methods
    private void Awake()
    {
        if(material == null)
        {
            material = gameObject.GetComponent<Collider>().material;
        }
        if(throwableScript == null)
        {
            throwableScript = gameObject.GetComponent<Throwable>();
        }
        GetOriginalValues();
        AddEventListeners();
    }

    /// <summary>
    /// Reset the values when the game is closed 
    /// </summary>
    private void OnApplicationQuit()
    {
        if (material != null)
        {
            if (CheckAnyValueDifference())
            {
                ResetValues();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Furniture"))
        {
            ChangeOtherValue(collision.collider);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Furniture"))
        {
            ResetOtherValue(collision.collider);
        }
    }
    #endregion

    private void AddEventListeners()
    {
        throwableScript.onPickUp.AddListener(ChangeSelfValue);
        throwableScript.onDetachFromHand.AddListener(ResetValues);
    }

    private void GetOriginalValues()
    {
        if (material != null)
        {
            OriginalDynamicFriction = material.dynamicFriction;
            OriginalStaticFriction = material.staticFriction;
            OriginalBounciness = material.bounciness;
            OriginalFrictionCombine = material.frictionCombine;
            OriginalBounceCombine = material.bounceCombine;
        }
    }

    private bool CheckAnyValueDifference()
    {
        return DynamicFriction != material.dynamicFriction 
                                    || StaticFriction != material.staticFriction 
                                    || Bounciness != material.bounciness 
                                    || FrictionCombine != material.frictionCombine 
                                    || BounceCombine != material.bounceCombine;
    }

    /// <summary>
    /// If the values requested are different than the values the material has, it will set them to the requested values.
    /// Will also use the original values if any are set to -1.
    /// </summary>
    public void ChangeSelfValue()
    {
        if (material != null)
        {
            material.dynamicFriction = DynamicFriction == material.dynamicFriction || DynamicFriction == -1 ? material.dynamicFriction : DynamicFriction;
            material.staticFriction = StaticFriction == material.staticFriction || StaticFriction == -1 ? material.staticFriction : StaticFriction;
            material.bounciness = Bounciness == material.bounciness || Bounciness == -1 ? material.bounciness : Bounciness;
            if(ChangeFrictionCombine)
            {
                material.frictionCombine = FrictionCombine == material.frictionCombine ? material.frictionCombine : FrictionCombine;
            }
            if(ChangeBounceCombine)
            {
                material.bounceCombine = BounceCombine == material.bounceCombine ? material.bounceCombine : BounceCombine;
            }
        }
    }

    private void ChangeOtherValue(Collider c)
    {
        OtherColliderOriginalBounciness = c.material.bounciness;
        c.material.bounciness = 0;
    }

    private void ResetOtherValue(Collider c)
    {
        c.material.bounciness = OtherColliderOriginalBounciness;
    }

    public void ResetValues()
    {
        if (material != null)
        {
            Debug.Log(string.Format("Resetting values for physics material {0} on object {1}", material.name, gameObject.name));
            material.dynamicFriction = OriginalDynamicFriction;
            material.staticFriction = OriginalStaticFriction;
            material.bounciness = OriginalBounciness;
            material.frictionCombine = OriginalFrictionCombine;
            material.bounceCombine = OriginalBounceCombine;
        }
    }

    

   
}
