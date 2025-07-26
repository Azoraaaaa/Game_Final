using UnityEngine;

public class CanvasActivator : MonoBehaviour
{
    [ContextMenu("Activate All Children")]
    public void ActivateAllChildren()
    {
        ActivateAllChildrenRecursive(transform);
        Debug.Log($"CanvasActivator: Activated all children of {gameObject.name}");
    }

    private void ActivateAllChildrenRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(true);
            ActivateAllChildrenRecursive(child);
        }
    }
} 