using UnityEngine;
using TMPro;

public class SceneSetup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    void Start()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.SetTimerText(timerText);
        }
    }
}
