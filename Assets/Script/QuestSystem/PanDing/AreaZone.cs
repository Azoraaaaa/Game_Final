using UnityEngine;

public class AreaZone : MonoBehaviour
{
    [Header("ºÏ≤‚…Ë÷√")]
    public string requiredTag = "PuzzleItem";
    public AreaManager areaManager;

    [Header("µ∆π‚…Ë÷√")]
    public Light spotLight;
    private Color originalColor;
    public Color activeColor = Color.red;

    [HideInInspector] public bool isCorrect = false;

    void Start()
    {
        if (spotLight != null)
        {
            originalColor = spotLight.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isCorrect = true;
            if (spotLight != null)
            {
                spotLight.color = activeColor;
                spotLight.enabled = true;
            }
            areaManager.CheckAllZones();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isCorrect = false;
            if (spotLight != null)
            {
                spotLight.color = originalColor;
            }
            areaManager.CheckAllZones();
        }
    }
}
