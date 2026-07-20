using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransitionAnimation : MonoBehaviour
{
    public Animator transition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        // if (SceneController.isSceneChanging)
        // {
        //     Debug.LogWarning("Next level load blocked by flag.");
        //     return;
        // }

        Debug.Log("Starting next level load.");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }


    IEnumerator LoadLevel(int levelIndex)
    {
        SceneController.isSceneChanging = true;

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex);
    }
}
