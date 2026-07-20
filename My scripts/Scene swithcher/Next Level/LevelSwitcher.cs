using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [Tooltip("Tag to detect for triggering level switch")]
    public string playerTag = "Player"; // Editable in Inspector

    private LevelTransitionAnimation script;

    void Start()
    {
        script = FindFirstObjectByType<LevelTransitionAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && script != null && Input.GetKeyDown(KeyCode.Q))
        {
            // Debug.Log("Next collided");
            script.LoadNextLevel(); // direct call
        }
    }
}
