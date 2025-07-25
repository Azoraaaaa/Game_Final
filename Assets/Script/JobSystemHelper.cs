using UnityEngine;

public class JobSystemHelper : MonoBehaviour
{
    private void OnEnable()
    {
        // 强制完成所有jobs
        Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobDebuggerEnabled = false;
    }

    private void OnDisable()
    {
        Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobDebuggerEnabled = true;
    }

    void OnApplicationQuit()
    {
        // 确保在退出时清理
        Unity.Jobs.LowLevel.Unsafe.JobsUtility.JobDebuggerEnabled = true;
    }
} 