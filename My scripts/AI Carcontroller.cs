using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class AICarController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform waypointGroup;
    public float stopDistance = 2f;

    [Header("Movement")]
    public float speed = 10f;
    public float turnSpeed = 5f;

    [Header("Blocking (Raycasts)")]
    [Tooltip("Extra distance beyond stopDistance to scan for cars")]
    public float blockMargin = 0.5f;
    [Tooltip("Forward offset from car center (bumper)")]
    public float frontOffset = 1f;
    [Tooltip("Height of the ray above ground")]
    public float rayHeight = 0.5f;
    [Tooltip("Half-width spacing for side rays")]
    public float sideOffset = 0.5f;

    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;

    private TrafficLight currentLight;
    private bool waitingAtRed = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;                           // ← now dynamic
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        foreach (Transform wp in waypointGroup)
            waypoints.Add(wp);
        if (waypoints.Count == 0)
            Debug.LogError("[AICar] No waypoints assigned!");
    }

    void FixedUpdate()
    {
        // 1) stop for red light
        if (waitingAtRed)
        {
            if (currentLight.CurrentColor != TrafficLight.LightColor.Red)
                waitingAtRed = false;
            else
                return;
        }

        // 2) stop for car ahead
        if (IsCarBlocking()) return;

        // 3) drive
        DriveToNextWaypoint();
    }

    private void DriveToNextWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector3 toTarget = target.position - rb.position;
        Vector3 dir = toTarget.normalized;

        rb.MoveRotation(Quaternion.Slerp(rb.rotation,
                                         Quaternion.LookRotation(dir),
                                         turnSpeed * Time.fixedDeltaTime));
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);

        if (toTarget.magnitude < stopDistance)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    private bool IsCarBlocking()
    {
        float checkDist = stopDistance + blockMargin;
        Vector3 forward = transform.forward;
        Vector3 baseOrigin = transform.position
                             + Vector3.up * rayHeight
                             + forward * frontOffset;

        // center, right, left beams
        Vector3[] offsets = {
            Vector3.zero,
            transform.right * sideOffset,
            -transform.right * sideOffset
        };

        foreach (var off in offsets)
        {
            Vector3 origin = baseOrigin + off;
            Debug.DrawRay(origin, forward * checkDist, Color.green);

            if (Physics.Raycast(origin, forward, out RaycastHit hit, checkDist))
            {
                var otherCar = hit.collider.GetComponent<AICarController>();
                if (otherCar != null && otherCar != this)
                {
                    Debug.DrawRay(origin, forward * checkDist, Color.red);
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other) => HandleLight(other);
    private void OnTriggerStay(Collider other)  => HandleLight(other);
    private void OnTriggerExit(Collider other)
    {
        var tl = other.GetComponentInParent<TrafficLight>();
        if (tl != null)
        {
            waitingAtRed = false;
            currentLight = null;
            Debug.Log("[AICar] Exited light zone");
        }
    }

    private void HandleLight(Collider other)
    {
        var tl = other.GetComponentInParent<TrafficLight>();
        if (tl == null) return;

        currentLight = tl;
        if (tl.CurrentColor == TrafficLight.LightColor.Red)
        {
            waitingAtRed = true;
            Debug.Log("[AICar] Detected RED light—stopping");
        }
    }
}
