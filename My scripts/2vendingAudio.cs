using UnityEngine;
using UnityEngine.SceneManagement;

public class OverAudio : MonoBehaviour
{
    public float triggerRadius = 3f;       // Radius in XZ plane
    public AudioSource audioSource;        // AudioSource on this object
    public string playerTag = "Player";    // Tag of player object

    private Transform playerTransform;
    private bool hasPlayed = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform == null || hasPlayed)
            return;

        // Check XZ distance only (ignore Y)
        Vector3 toPlayer = playerTransform.position - transform.position;
        Vector2 toPlayerXZ = new Vector2(toPlayer.x, toPlayer.z);

        if (toPlayerXZ.magnitude <= triggerRadius)
        {
            audioSource.Play();
            hasPlayed = true;
            Debug.Log("Proximity audio triggered!");
            SceneManager.LoadScene(2);
        }
    }
}
