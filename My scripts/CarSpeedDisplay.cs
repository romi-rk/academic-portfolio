using UnityEngine;
using TMPro;

public class CarSpeedDisplay : MonoBehaviour
{
    public Rigidbody carRigidbody; // Assign your car's Rigidbody in the inspector
    public TextMeshProUGUI speedText;

    void Update()
    {
        if (carRigidbody != null && speedText != null)
        {
            float speed = carRigidbody.linearVelocity.magnitude * 3.6f; // Convert m/s to km/h
            speedText.text = Mathf.RoundToInt(speed) + " km/h";

        }
    }
}
