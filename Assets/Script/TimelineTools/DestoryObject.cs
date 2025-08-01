using UnityEngine;

public class DestoryObject : MonoBehaviour
{
    public GameObject targetToHide;

    public void Hide()
    {
        if (targetToHide != null)
        {
            targetToHide.SetActive(false);
        }
    }
}
