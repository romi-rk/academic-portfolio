using UnityEngine;
using UnityEngine.UI; // For Unity UI Text

public class PlayerCollisionDetector : MonoBehaviour
{
    public GameObject uiParent; // Assign the parent of the Text object (e.g., a Canvas or panel)
    public string carTag = "Car"; // The tag assigned to car objects
    public string message = "You've been hit by a car!";
    public float displayDuration = 2f;

    private Text displayText;

    private void Start()
    {
        if (uiParent != null)
        {
            displayText = uiParent.GetComponentInChildren<Text>(true); // true = include inactive children
            if (displayText == null)
            {
                Debug.LogWarning("No Text component found in children of uiParent.");
            }
        }
        else
        {
            Debug.LogWarning("uiParent not assigned.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(carTag) && displayText != null)
        {
            displayText.text = message;
            displayText.enabled = true;
            CancelInvoke("HideText");
            Invoke("HideText", displayDuration);
        }
    }

    private void HideText()
    {
        if (displayText != null)
        {
            displayText.enabled = false;
        }
    }
}
