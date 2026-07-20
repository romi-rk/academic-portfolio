using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    [Tooltip("Tag that triggers a level reset when entering the trigger")]
    public string resetTag = "LevelReset"; // Editable in Inspector

    private LevelResetAnimation script;
    
    void Start()
    {
        script = FindFirstObjectByType<LevelResetAnimation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(resetTag) && script != null)
        {
            Debug.Log("reset collided");
            script.ResetLevel(); // direct call
        }
    }
}
