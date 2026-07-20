using UnityEngine;

public class PlayerCarCollisionTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        bool isPlayer = gameObject.CompareTag("Player");
        bool hitCar = collision.collider.CompareTag("laCar");

        if (isPlayer && hitCar)
        {
            Debug.Log("Collision Detected");
            ShowOnPlayerCarCollision.TriggerAppearance();
        }
    }
}
