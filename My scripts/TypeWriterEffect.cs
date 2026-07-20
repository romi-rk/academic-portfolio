using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [Header("UI Reference")]
    [Tooltip("The TextMeshProUGUI component where the text will be displayed.")]
    public TextMeshProUGUI textComponent;

    [Header("Text & Speed")]
    [TextArea]
    [Tooltip("The full string to be revealed progressively.")]
    public string fullText;
    [Tooltip("Time in seconds between each character (will be recalculated if syncing to audio).")]
    public float delay = 0.05f;
    [Tooltip("If enabled, the character delay is recalculated to match the audio clip length.")]
    public bool syncToAudio = true;

    [Header("Audio")]
    [Tooltip("The AudioSource that will play your voice-over clip.")]
    public AudioSource voiceSource;
    [Tooltip("Time in seconds to wait before starting the audio.")]
    public float audioDelay = 0.5f;

    [Header("Scene Settings")]
    [Tooltip("Enable to automatically load the next scene when text completes.")]
    public bool loadNextSceneOnComplete = true;
    [Tooltip("Extra seconds to wait after text finishes before loading.")]
    public float postLoadDelay = 0.5f;

    private string currentText = "";

    void Start()
    {
        // If syncing to audio, recalc delay so text & audio finish together
        if (syncToAudio && voiceSource != null && voiceSource.clip != null)
        {
            float clipLength = voiceSource.clip.length;
            delay = clipLength / fullText.Length;
            voiceSource.PlayDelayed(audioDelay);
        }

        // Begin the typewriter effect
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        // Text is fully displayed
        if (loadNextSceneOnComplete)
        {
            yield return new WaitForSeconds(postLoadDelay);
            LoadNextScene();
        }
    }

    void Update()
    {
        // Allow skipping to the end of the text (and stopping audio)
        if (Input.GetKeyDown(KeyCode.Space) && textComponent.text != fullText)
        {
            StopAllCoroutines();
            textComponent.text = fullText;

            if (voiceSource != null && voiceSource.isPlaying)
                voiceSource.Stop();

            if (loadNextSceneOnComplete)
                LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("Next scene index exceeds available scenes in Build Settings.");
        }
    }
}
