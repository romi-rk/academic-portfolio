using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(Collider))]
public class NPCController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("How far (in world units) to pick random patrol points around the NPC")]
    public float patrolRange = 50f;
    [Tooltip("How close is 'close enough' to a patrol point before picking a new one")]
    public float stoppingDistance = 1f;

    [Header("Animation Settings")]
    [Tooltip("Name of the bool param that drives Idle↔Walking")]
    public string runningBool = "Running";
    [Tooltip("Trigger to fire your Fall animation")]
    public string fallTrigger = "Fall";
    [Tooltip("Trigger to fire your GetUp animation")]
    public string getUpTrigger = "GetUp";
    [Tooltip("Exact names of your Animator clip states (in Base Layer)")]
    public string fallStateName  = "fall";
    public string getUpStateName = "getUp";
    [Range(0f, 1f), Tooltip("When to leave Fall/GetUp (e.g. .9 = 90% through)")]
    public float exitTimePercent = 0.9f;

    private NavMeshAgent agent;
    private Animator     anim;
    private Collider     col;

    private Vector3   nextDest;
    private bool      hasDest;
    private bool      isHit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim  = GetComponent<Animator>();
        col   = GetComponent<Collider>();

        // You want your NPC collider marked as "Is Trigger" so OnTriggerEnter fires:
        if (!col.isTrigger)
            Debug.LogWarning("NPCController: Collider should be a Trigger!");

        // begin patrol immediately
        PickNewDestination();
    }

    void Update()
    {
        if (isHit)
            return;

        // If we've reached (or never set) a patrol point, pick a new one
        if (!hasDest ||
            (agent.remainingDistance <= stoppingDistance && !agent.pathPending))
        {
            PickNewDestination();
        }

        // feed the Running bool so your Idle↔Walking transitions fire
        anim.SetBool(runningBool, agent.velocity.magnitude > 0.1f);
    }

    private void PickNewDestination()
    {
        // Pick a random point in a sphere
        Vector3 random = transform.position + Random.insideUnitSphere * patrolRange;

        // Sample the NavMesh to find a nearby valid point
        if (NavMesh.SamplePosition(random, out NavMeshHit hit, patrolRange, NavMesh.AllAreas))
        {
            nextDest = hit.position;
            agent.SetDestination(nextDest);
            hasDest = true;
        }
        else
        {
            // If sampling failed (rare), try again next frame
            hasDest = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isHit) return;
        if (other.CompareTag("Car"))
        {
            isHit = true;
            StartCoroutine(FallAndGetUp());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            // allow being hit again once the car leaves
            isHit = false;
        }
    }

    private IEnumerator FallAndGetUp()
    {
        // 1) stop patrolling
        agent.isStopped = true;

        // 2) play Fall
        anim.SetTrigger(fallTrigger);
        yield return WaitForStateAndTime(fallStateName, exitTimePercent);

        // 3) play GetUp
        anim.SetTrigger(getUpTrigger);
        yield return WaitForStateAndTime(getUpStateName, exitTimePercent);

        // 4) resume patrol
        agent.isStopped = false;
        hasDest = false;           // force a new destination next Update
    }

    private IEnumerator WaitForStateAndTime(string stateName, float normTime)
    {
        // wait until the Animator is in the given state AND at least normTime through
        while (true)
        {
            var st = anim.GetCurrentAnimatorStateInfo(0);
            if (st.IsName(stateName) && st.normalizedTime >= normTime)
                yield break;
            yield return null;
        }
    }
}