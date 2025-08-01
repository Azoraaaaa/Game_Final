using UnityEngine;

public class TimelineConditionDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    public TimelineCondition timelineCondition;
    public bool showDebugInfo = true;
    public KeyCode resetKey = KeyCode.R;
    public KeyCode testPlayKey = KeyCode.T;

    private void Start()
    {
        if (timelineCondition == null)
        {
            timelineCondition = GetComponent<TimelineCondition>();
        }
    }

    private void Update()
    {
        if (timelineCondition == null) return;

        // 按R键重置状态
        if (Input.GetKeyDown(resetKey))
        {
            timelineCondition.ResetState();
            Debug.Log("=== Timeline Condition Reset ===");
        }

        // 按T键测试播放
        if (Input.GetKeyDown(testPlayKey))
        {
            Debug.Log("=== Testing Timeline Play ===");
            bool success = timelineCondition.TryPlayTimeline();
            Debug.Log($"Timeline play result: {success}");
        }
    }

    private void OnGUI()
    {
        if (!showDebugInfo || timelineCondition == null) return;

        GUILayout.BeginArea(new Rect(10, 10, 400, 300));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("=== Timeline Condition Debug ===", GUI.skin.box);
        GUILayout.Space(5);
        
        // 显示当前状态信息
        string stateInfo = timelineCondition.GetCurrentStateInfo();
        GUILayout.Label($"Current State: {stateInfo}");
        
        GUILayout.Space(10);
        
        // 显示所有状态信息
        GUILayout.Label("All States:", GUI.skin.box);
        for (int i = 0; i < timelineCondition.timelineStates.Length; i++)
        {
            var state = timelineCondition.timelineStates[i];
            string status = state.hasPlayed ? "✓ Played" : "○ Not Played";
            string questInfo = !string.IsNullOrEmpty(state.requiredQuestId) 
                ? $"Quest: {state.requiredQuestId} (Complete: {state.isQuestComplete})" 
                : "No Quest";
            
            GUILayout.Label($"State {i}: {state.timeline?.name} - {status}");
            GUILayout.Label($"  {questInfo}");
        }
        
        GUILayout.Space(10);
        
        // 控制按钮
        GUILayout.Label("Controls:", GUI.skin.box);
        GUILayout.Label($"Press {resetKey} to Reset State");
        GUILayout.Label($"Press {testPlayKey} to Test Play");
        
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
} 