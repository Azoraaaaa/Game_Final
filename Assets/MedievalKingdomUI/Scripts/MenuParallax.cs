using UnityEngine;

public class UIParallax : MonoBehaviour
{
    public RectTransform uiElement;
    public float offsetMultiplier = 50f;  // 控制偏移幅度，UI单位用像素，值可以调大一点
    public float smoothTime = 0.3f;

    private Vector2 startPosition;
    private Vector2 velocity;

    void Start()
    {
        if (uiElement == null)
            uiElement = GetComponent<RectTransform>();

        startPosition = uiElement.anchoredPosition;
    }

    void Update()
    {
        // 鼠标位置转归一化 -0.5 到 0.5 中心对齐
        Vector2 normalizedMousePos = (Input.mousePosition / new Vector2(Screen.width, Screen.height)) - new Vector2(0.5f, 0.5f);
        Vector2 targetPosition = startPosition + (normalizedMousePos * offsetMultiplier);

        // 平滑移动
        uiElement.anchoredPosition = Vector2.SmoothDamp(uiElement.anchoredPosition, targetPosition, ref velocity, smoothTime);
    }
}
