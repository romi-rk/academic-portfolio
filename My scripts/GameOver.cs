using UnityEngine;
using UnityEngine.SceneManagement;  // Required to load scenes

public class GameOver : MonoBehaviour
{
    public string carTag = "Car";

    [Header("Scene Transition")]
    public int sceneIndexToLoad = 4;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(carTag))
        {
            Debug.Log("colision detected !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            SceneManager.LoadScene(sceneIndexToLoad);
        }
    }
}
