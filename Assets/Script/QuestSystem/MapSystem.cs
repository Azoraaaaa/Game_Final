using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSystem : MonoBehaviour
{
    public static MapSystem Instance { get; private set; }
    
    [Header("Map References")]
    public GameObject mapPanel;
    public Transform questMarkersContainer;
    public GameObject questMarkerPrefab;
    
    [Header("Marker Icons")]
    public Sprite activeQuestIcon;
    public Sprite completedQuestIcon;
    public Sprite availableQuestIcon;
    
    private Dictionary<string, GameObject> questMarkers = new Dictionary<string, GameObject>();
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    // 添加任务标记
    public void AddQuestMarker(QuestData quest)
    {
        if (questMarkers.ContainsKey(quest.questID))
        {
            UpdateQuestMarker(quest, QuestStatus.InProgress);
            return;
        }
        
        GameObject marker = Instantiate(questMarkerPrefab, questMarkersContainer);
        questMarkers[quest.questID] = marker;
        
        // 设置标记位置
        SetMarkerPosition(marker, quest.questLocation);
        
        // 设置标记图标和颜色
        UpdateMarkerVisual(marker, QuestStatus.InProgress);
        
        Debug.Log($"Added quest marker for {quest.questID}");
    }
    
    // 更新任务标记
    public void UpdateQuestMarker(QuestData quest, QuestStatus status)
    {
        if (!questMarkers.ContainsKey(quest.questID))
        {
            AddQuestMarker(quest);
            return;
        }
        
        GameObject marker = questMarkers[quest.questID];
        UpdateMarkerVisual(marker, status);
        
        // 如果任务完成，可以选择移除标记或保持灰色
        if (status == QuestStatus.Completed)
        {
            // 可以选择延迟移除标记
            StartCoroutine(RemoveMarkerAfterDelay(quest.questID, 3f));
        }
    }
    
    // 移除任务标记
    public void RemoveQuestMarker(string questID)
    {
        if (questMarkers.ContainsKey(questID))
        {
            Destroy(questMarkers[questID]);
            questMarkers.Remove(questID);
        }
    }
    
    // 设置标记位置
    private void SetMarkerPosition(GameObject marker, Vector3 worldPosition)
    {
        // 将世界坐标转换为地图坐标
        Vector2 mapPosition = WorldToMapPosition(worldPosition);
        marker.GetComponent<RectTransform>().anchoredPosition = mapPosition;
    }
    
    // 更新标记视觉效果
    private void UpdateMarkerVisual(GameObject marker, QuestStatus status)
    {
        Image markerImage = marker.GetComponent<Image>();
        if (markerImage != null)
        {
            switch (status)
            {
                case QuestStatus.InProgress:
                    markerImage.sprite = activeQuestIcon;
                    markerImage.color = Color.yellow;
                    break;
                case QuestStatus.Completed:
                    markerImage.sprite = completedQuestIcon;
                    markerImage.color = Color.gray;
                    break;
                case QuestStatus.NotStarted:
                    markerImage.sprite = availableQuestIcon;
                    markerImage.color = Color.white;
                    break;
            }
        }
    }
    
    // 世界坐标转地图坐标
    private Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        // 这里需要根据您的地图系统实现具体的坐标转换
        // 简单示例：假设地图是1:1的比例
        return new Vector2(worldPosition.x, worldPosition.z);
    }
    
    // 延迟移除标记
    private System.Collections.IEnumerator RemoveMarkerAfterDelay(string questID, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveQuestMarker(questID);
    }
    
    // 显示/隐藏地图
    public void ToggleMap()
    {
        if (mapPanel != null)
        {
            mapPanel.SetActive(!mapPanel.activeSelf);
        }
    }
    
    // 清除所有标记
    public void ClearAllMarkers()
    {
        foreach (var marker in questMarkers.Values)
        {
            Destroy(marker);
        }
        questMarkers.Clear();
    }
} 