using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingTextOnTrigger : MonoBehaviour
{
    [Header("Canvas That Contains the Text Box")]
    public GameObject canvasObject;

    [Header("Text Settings")]
    [TextArea]
    public string messageToType = "Hello there! This is a typed message.";
    public float typingSpeed = 0.05f; // Time between characters

    [Header("Trigger Settings")]
    public string carTag = "laCar";

    private Text uiText;
    private TextMeshProUGUI tmpText;
    private bool hasTriggered = false;

    private void Start()
    {
        if (canvasObject != null)
        {
            // Try to find a Text or TMP component in the canvas
            uiText = canvasObject.GetComponentInChildren<Text>();
            tmpText = canvasObject.GetComponentInChildren<TextMeshProUGUI>();

            // Ensure text starts empty
            if (uiText != null) uiText.text = "";
            if (tmpText != null) tmpText.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag(carTag))
        {
            Debug.Log("Trigger entered by: " + other.name);
            hasTriggered = true;

            if (canvasObject != null && (uiText != null || tmpText != null))
            {
                StartCoroutine(TypeText());
            }
        }
    }

    private IEnumerator TypeText()
    {
        if (uiText != null)
        {
            uiText.text = "";
            foreach (char c in messageToType)
            {
                uiText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        else if (tmpText != null)
        {
            tmpText.text = "";
            foreach (char c in messageToType)
            {
                tmpText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}
