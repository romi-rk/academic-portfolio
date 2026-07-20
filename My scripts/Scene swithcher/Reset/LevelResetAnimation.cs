using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelResetAnimation : MonoBehaviour
{
    public Animator transition;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
    }

    public void ResetLevel()
    {
        // if (SceneController.isSceneChanging)
        // {
        //     Debug.LogWarning("Scene reset blocked by flag.");
        //     return;
        // }

        Debug.Log("Starting scene reset.");
        StartCoroutine(LevelReset());
    }

    IEnumerator LevelReset()
    {
        SceneController.isSceneChanging = true;

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
