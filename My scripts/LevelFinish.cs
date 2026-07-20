using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelFinish : MonoBehaviour
{
    public GameObject transitionUI;         // UI for normal level transition
    public GameObject gameOverUI;           // UI shown when no more levels
    public TMP_Text transitionText;         // Text shown during level transition
    public TMP_Text gameOverText;           // Text shown when all levels completed
    public CanvasGroup canvasGroup;         // Fade control for transition UI
    public CanvasGroup gameOverCanvasGroup; // Fade control for game over UI
    public float delayBeforeNextLevel = 3f;
    public float fadeDuration = 1.5f;

    [Header("Audio Settings")]
    public AudioSource audioSource;         // AudioSource component to play sounds
    public AudioClip level1EndDialogue;     // Audio clip for Level 1 end dialogue

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandleLevelEnd());
        }
    }

    private IEnumerator HandleLevelEnd()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Play Level 1 end dialogue audio before transition (only if this is Level 1)
        if (currentSceneIndex == 0 && level1EndDialogue != null && audioSource != null)
        {
            audioSource.clip = level1EndDialogue;
            audioSource.Play();
            // Wait until the audio finishes
            yield return new WaitUntil(() => !audioSource.isPlaying);
        }

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Show normal transition UI
            if (transitionUI != null)
            {
                transitionUI.SetActive(true);
                if (transitionText != null)
                {
                    transitionText.text = "🎉 Congratulations!\nLoading next level...";
                }

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                }
            }

            yield return new WaitForSeconds(delayBeforeNextLevel);

            if (canvasGroup != null)
            {
                float t = 0f;
                while (t < fadeDuration)
                {
                    t += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                    yield return null;
                }
                canvasGroup.alpha = 0f;
            }

            if (transitionUI != null)
            {
                transitionUI.SetActive(false);
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Show game over UI
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
                if (gameOverText != null)
                {
                    gameOverText.text = "🏁 You finished all levels!\nThanks for playing!";
                }

                if (gameOverCanvasGroup != null)
                {
                    gameOverCanvasGroup.alpha = 1f;
                }
            }

            // Optional delay or fade-out for gameOver UI
            yield return new WaitForSeconds(delayBeforeNextLevel);

            if (gameOverCanvasGroup != null)
            {
                float t = 0f;
                while (t < fadeDuration)
                {
                    t += Time.deltaTime;
                    gameOverCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                    yield return null;
                }
                gameOverCanvasGroup.alpha = 0f;
            }

            Debug.Log("No more levels. Game finished!");
            // SceneManager.LoadScene("MainMenu"); // Uncomment if you have a menu scene
        }
    }
}
