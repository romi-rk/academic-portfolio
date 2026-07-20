using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class TrafficLightManager : MonoBehaviour
{
    [Header("Forward (Through) Lights")]
    public TrafficLight northForward;
    public TrafficLight eastForward;
    public TrafficLight southForward;
    public TrafficLight westForward;

    [Header("Left-Turn Lights")]
    public TrafficLight northLeft;
    public TrafficLight eastLeft;
    public TrafficLight southLeft;
    public TrafficLight westLeft;

    [Header("Through-Traffic Timing (seconds)")]
    [Range(5f, 60f)]
    [Tooltip("How long through-traffic stays green")]
    public float throughGreenDuration = 10f;
    [Range(1f, 10f)]
    [Tooltip("How long through-traffic stays yellow")]
    public float throughYellowDuration = 3f;

    [Header("Left-Turn Timing (seconds)")]
    [Range(3f, 30f)]
    [Tooltip("How long left-turn arrow stays green")]
    public float leftGreenDuration = 5f;
    [Range(1f, 5f)]
    [Tooltip("How long left-turn arrow stays yellow")]
    public float leftYellowDuration = 2f;

    private void Start()
    {
        // sanity‐check your durations
        if (throughGreenDuration <= 0f || throughYellowDuration <= 0f ||
            leftGreenDuration <= 0f || leftYellowDuration <= 0f)
        {
            Debug.LogWarning($"[{name}] All durations should be > 0.", this);
        }

        StartCoroutine(CyclePhases());
    }

    private IEnumerator CyclePhases()
    {
        while (true)
        {
            // --- 1) North/South Left-Turn Phase ---
            SetAllRed();
            SetPair(northLeft, southLeft, TrafficLight.LightColor.Green);
            yield return new WaitForSeconds(leftGreenDuration);
            SetPair(northLeft, southLeft, TrafficLight.LightColor.Yellow);
            yield return new WaitForSeconds(leftYellowDuration);

            // --- 2) North/South Through Phase ---
            SetAllRed();
            SetPair(northForward, southForward, TrafficLight.LightColor.Green);
            yield return new WaitForSeconds(throughGreenDuration);
            SetPair(northForward, southForward, TrafficLight.LightColor.Yellow);
            yield return new WaitForSeconds(throughYellowDuration);

            // --- 3) East/West Left-Turn Phase ---
            SetAllRed();
            SetPair(eastLeft, westLeft, TrafficLight.LightColor.Green);
            yield return new WaitForSeconds(leftGreenDuration);
            SetPair(eastLeft, westLeft, TrafficLight.LightColor.Yellow);
            yield return new WaitForSeconds(leftYellowDuration);

            // --- 4) East/West Through Phase ---
            SetAllRed();
            SetPair(eastForward, westForward, TrafficLight.LightColor.Green);
            yield return new WaitForSeconds(throughGreenDuration);
            SetPair(eastForward, westForward, TrafficLight.LightColor.Yellow);
            yield return new WaitForSeconds(throughYellowDuration);
        }
    }

    /// <summary>
    /// Turns *every* light to red.
    /// </summary>
    private void SetAllRed()
    {
        // through
        SetPair(northForward, southForward, TrafficLight.LightColor.Red);
        SetPair(eastForward,  westForward,  TrafficLight.LightColor.Red);
        // left
        SetPair(northLeft,    southLeft,    TrafficLight.LightColor.Red);
        SetPair(eastLeft,     westLeft,     TrafficLight.LightColor.Red);
    }

    /// <summary>
    /// Helper to set two opposite signals to the same color.
    /// </summary>
    private void SetPair(TrafficLight t1, TrafficLight t2, TrafficLight.LightColor color)
    {
        if (t1 != null) t1.SetColor(color);
        if (t2 != null) t2.SetColor(color);
    }
}
