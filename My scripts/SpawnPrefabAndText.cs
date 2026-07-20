using System.Collections;
using UnityEngine;

public class SpawnPrefabAndText : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefabToSpawn;
    public Transform spawnLocation;

    [Header("Tag Settings")]
    public string carTag = "laCar";

    [Header("UI Text GameObjects")]
    [Tooltip("The GameObject holding the text to enable temporarily.")]
    public GameObject textHolderToEnable;

    [Tooltip("Other text GameObjects to disable during the delay.")]
    public GameObject[] textHoldersToDisable;

    [Header("UI Display Timing")]
    public float textDisplayDuration = 3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(carTag))
        {
            Debug.Log("Collision !!!");

            if (prefabToSpawn != null && spawnLocation != null)
            {
                Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            }

            if (textHolderToEnable != null)
            {
                StartCoroutine(HandleUIText());
            }

            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
    }

    private IEnumerator HandleUIText()
    {
        // Disable other text holders
        foreach (GameObject obj in textHoldersToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Enable the main text holder
        textHolderToEnable.SetActive(true);

        // Wait for specified duration
        yield return new WaitForSeconds(textDisplayDuration);

        // Disable the main text holder again
        textHolderToEnable.SetActive(false);
    }
}
