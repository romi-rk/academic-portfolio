using UnityEngine;
using UnityEngine.UI;

public class CarCrashText : MonoBehaviour
{
    public Text messageText; // Assign this in the Inspector

    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("Hit by a car!");
            Time.timeScale = 0f; // Freeze game
            if (messageText != null)
            {
                messageText.text = "You were hit by a car!";
                messageText.gameObject.SetActive(true);
            }
        }
    }
}
