using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime_s;

    string originalText;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this GameObject between scenes
    }

    void Start()
    {
        if (timerText != null)
        {
            originalText = timerText.text;
        }
    }

    void Update()
    {
        if (remainingTime_s > 0)
        {
            remainingTime_s -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime_s / 60);
            int seconds = Mathf.FloorToInt(remainingTime_s % 60);

            if (timerText != null)
            {
                timerText.text = string.Format("{0} {1:00}:{2:00}", originalText, minutes, seconds);
            }
        }
        else
        {
            if (timerText != null)
            {
                timerText.text = "<color=red>You're late for class!!!</color>";
            }
        }
    }

    public float GetRemainingTime()
    {
        return remainingTime_s;
    }

    public void SetTimerText(TextMeshProUGUI newTextComponent)
    {
        timerText = newTextComponent;
        originalText = timerText.text;
    }
}

