using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class HitByCar : MonoBehaviour
{
    private Animator     _anim;
    private NavMeshAgent _agent;
    private Collider     _collider;
    private bool         _isHit;

    void Awake()
    {
        _anim     = GetComponent<Animator>();
        _agent    = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();

        if (_collider == null || !_collider.isTrigger)
            Debug.LogWarning("Player collider should be marked IsTrigger for OnTriggerEnter to fire.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (_isHit) return;

        if (other.CompareTag("Car"))
        {
            _isHit = true;
            StartCoroutine(DoFallAndGetUp());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("Car exited trigger zone – ready for next hit");
            _isHit = false;
        }
    }

    private IEnumerator DoFallAndGetUp()
    {
        // Stop movement
        _agent.isStopped       = true;
        // _agent.updatePosition  = false;
        // _agent.updateRotation  = false;

        // Play fall
        _anim.SetTrigger("Fall");

        // Wait until 90% through the Fall clip
        while (true)
        {
            var state = _anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Fall") && state.normalizedTime >= 0.9f)
                break;
            yield return null;
        }

        // Play get-up
        _anim.SetTrigger("GetUp");

        // Wait until 90% through the GetUp clip
        while (true)
        {
            var state = _anim.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("GetUp") && state.normalizedTime >= 0.9f)
                break;
            yield return null;
        }

        // Restore movement
        _agent.updatePosition = true;
        // _agent.updateRotation = true;
        // _agent.isStopped      = false;

        // (Optional) clear _isHit here if you want it immediately—
        // OnTriggerExit will also reset it once the car leaves.
        _isHit = false;
    }
}