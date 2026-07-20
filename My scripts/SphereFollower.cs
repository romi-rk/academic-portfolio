using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereFollower : MonoBehaviour
{
    public Transform player;       // Assign your player object in the Inspector
    public float followSpeed = 5f; // Speed at which the sphere follows
    public float stopDistance = 1f; // Distance at which sphere stops moving toward the player

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position);
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            direction.Normalize();
            rb.MovePosition(rb.position + direction * followSpeed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player)
        {
            Debug.Log("Sphere collided with the player!");
            // Add additional behavior here (e.g., damage, bounce, stop)
        }
    }
}
