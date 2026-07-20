using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightColor { Red, Yellow, Green }

    [Header("Renderers")]
    public Renderer redRenderer;
    public Renderer yellowRenderer;
    public Renderer greenRenderer;

    [Header("Materials")]
    public Material redOn;
    public Material yellowOn;
    public Material greenOn;
    public Material off;

    public LightColor CurrentColor { get; private set; }

    public void SetColor(LightColor color)
    {
        Debug.Log($"[{gameObject.name}] Set to {color}");

        CurrentColor = color;

        if (redRenderer) redRenderer.material = new Material(off);
        if (yellowRenderer) yellowRenderer.material = new Material(off);
        if (greenRenderer) greenRenderer.material = new Material(off);

        switch (color)
        {
            case LightColor.Red:
                if (redRenderer) redRenderer.material = new Material(redOn);
                break;
            case LightColor.Yellow:
                if (yellowRenderer) yellowRenderer.material = new Material(yellowOn);
                break;
            case LightColor.Green:
                if (greenRenderer) greenRenderer.material = new Material(greenOn);
                break;
        }
    }
}
