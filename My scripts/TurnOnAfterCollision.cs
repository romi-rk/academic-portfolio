using UnityEngine;

public class ActivateOnPlayerCarCollision : MonoBehaviour
{
    [Tooltip("The object to activate after the Player collides with a Car.")]
    public GameObject objectToActivate;

    private bool hasActivated = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Make sure the object with this script has the "Player" tag
        if (CompareTag("Player") && collision.collider.CompareTag("Car") && !hasActivated)
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                hasActivated = true;
            }
            else
            {
                Debug.LogWarning("No object assigned to activate.");
            }
        }
    }
}
