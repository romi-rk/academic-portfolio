using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragDollOn : MonoBehaviour
{
    public BoxCollider MainCollider;
    public GameObject thisGuysRig;
    public Animator thisGuysAnimator;

    void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
          isOnRagdoll();
    }

    Collider[] RagdollColiders;
    Rigidbody[] LimbsRigitbodies;


    void isOnRagdoll()
    {
        RagdollModeOn();
    }
    void GetRagdollBits()
    {
        RagdollColiders = thisGuysRig.GetComponentsInChildren<Collider>();
        LimbsRigitbodies = thisGuysRig.GetComponentsInChildren<Rigidbody>();
    }
    void RagdollModeOn()
    {
        foreach (Collider col in RagdollColiders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in LimbsRigitbodies)
        {
            rb.isKinematic = false;
        }
        thisGuysAnimator.enabled = false;
        MainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void RagdollModeOff()
    {
        foreach (Collider col in RagdollColiders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in LimbsRigitbodies)
        {
            rb.isKinematic = true;
        }
        thisGuysAnimator.enabled = true;
        MainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}