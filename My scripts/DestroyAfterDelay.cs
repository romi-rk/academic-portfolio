using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
    [SerializeField] public float delay = 5f;

    void Start()
    {
        Invoke("DisableTextObject", delay);
    }

    void DisableTextObject()
    {
        gameObject.SetActive(false);
    }
}
