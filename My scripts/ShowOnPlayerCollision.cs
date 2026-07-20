using UnityEngine;

public class ShowOnPlayerCarCollision : MonoBehaviour
{
    private static bool collisionOccurred = false;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (collisionOccurred && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    // Call this method externally when collision is detected
    public static void TriggerAppearance()
    {
        collisionOccurred = true;
    }
}
