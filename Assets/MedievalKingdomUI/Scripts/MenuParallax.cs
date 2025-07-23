using UnityEngine;

public class UIParallax : MonoBehaviour
{
    public RectTransform uiElement;
    public float offsetMultiplier = 50f;  // ����ƫ�Ʒ��ȣ�UI��λ�����أ�ֵ���Ե���һ��
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
        // ���λ��ת��һ�� -0.5 �� 0.5 ���Ķ���
        Vector2 normalizedMousePos = (Input.mousePosition / new Vector2(Screen.width, Screen.height)) - new Vector2(0.5f, 0.5f);
        Vector2 targetPosition = startPosition + (normalizedMousePos * offsetMultiplier);

        // ƽ���ƶ�
        uiElement.anchoredPosition = Vector2.SmoothDamp(uiElement.anchoredPosition, targetPosition, ref velocity, smoothTime);
    }
}
