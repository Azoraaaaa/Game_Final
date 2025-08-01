using UnityEngine;
using UnityEditor;

/// <summary>
/// Dialogue node migration tool
/// Used to migrate existing dialogue nodes from single sentence field to sentences array format
/// Also supports multi-speaker functionality migration
/// </summary>
public class DialogueNodeMigrationTool : MonoBehaviour
{
    [Header("Migration Settings")]
    [Tooltip("Whether to automatically migrate all dialogue nodes")]
    public bool autoMigrateAll = false;
    
    [Tooltip("Dialogue nodes to migrate")]
    public DialogueNode[] nodesToMigrate;
    
    private void Start()
    {
        if (autoMigrateAll)
        {
            MigrateAllDialogueNodes();
        }
    }
    
    /// <summary>
    /// Migrate all dialogue nodes
    /// </summary>
    public void MigrateAllDialogueNodes()
    {
        // Find all dialogue node assets
        string[] guids = AssetDatabase.FindAssets("t:DialogueNode");
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DialogueNode node = AssetDatabase.LoadAssetAtPath<DialogueNode>(path);
            
            if (node != null)
            {
                MigrateDialogueNode(node);
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log("All dialogue nodes migration completed!");
    }
    
    /// <summary>
    /// Migrate a single dialogue node
    /// </summary>
    /// <param name="node">Dialogue node to migrate</param>
    public void MigrateDialogueNode(DialogueNode node)
    {
        if (node == null) return;
        
        // Check if already migrated
        if (node.sentences != null && node.sentences.Length > 0)
        {
            Debug.Log($"Dialogue node {node.name} has already been migrated, skipping.");
            return;
        }
        
        // Get original sentence content
        string oldSentence = "";
        
        // Use reflection to get private field (if exists)
        var field = typeof(DialogueNode).GetField("sentence", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            oldSentence = field.GetValue(node) as string;
        }
        
        // If can't get it, try using property
        if (string.IsNullOrEmpty(oldSentence))
        {
            oldSentence = node.sentence;
        }
        
        // Get original speakerName
        string oldSpeakerName = "";
        var speakerField = typeof(DialogueNode).GetField("speakerName", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (speakerField != null)
        {
            oldSpeakerName = speakerField.GetValue(node) as string;
        }
        
        // If can't get it, try using property
        if (string.IsNullOrEmpty(oldSpeakerName))
        {
            oldSpeakerName = node.speakerName;
        }
        
        // Migrate to new format
        if (!string.IsNullOrEmpty(oldSentence))
        {
            // Set default speaker
            node.defaultSpeakerName = oldSpeakerName;
            
            // Set sentences array
            node.sentences = new string[] { oldSentence };
            
            // Set speaker names array
            node.speakerNames = new string[] { "" }; // Empty string means use default speaker
            
            // Mark as modified
            EditorUtility.SetDirty(node);
            
            Debug.Log($"Dialogue node {node.name} migrated successfully!");
        }
        else
        {
            Debug.LogWarning($"Dialogue node {node.name} has no original sentence content, skipping migration.");
        }
    }
    
    /// <summary>
    /// Batch set speaker names
    /// </summary>
    /// <param name="speakerName">Speaker name to set</param>
    public void BatchSetSpeaker(string speakerName)
    {
        if (string.IsNullOrEmpty(speakerName))
        {
            Debug.LogWarning("Speaker name cannot be empty!");
            return;
        }
        
        string[] guids = AssetDatabase.FindAssets("t:DialogueNode");
        int count = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DialogueNode node = AssetDatabase.LoadAssetAtPath<DialogueNode>(path);
            
            if (node != null && node.sentences != null && node.sentences.Length > 0)
            {
                // Set default speaker
                node.defaultSpeakerName = speakerName;
                
                // Clear all custom speakers, use default speaker
                if (node.speakerNames != null)
                {
                    for (int i = 0; i < node.speakerNames.Length; i++)
                    {
                        node.speakerNames[i] = "";
                    }
                }
                
                EditorUtility.SetDirty(node);
                count++;
            }
        }
        
        AssetDatabase.SaveAssets();
        Debug.Log($"Successfully set speaker for {count} dialogue nodes: {speakerName}");
    }
}

#if UNITY_EDITOR
/// <summary>
/// Editor tools, providing migration menu
/// </summary>
public static class DialogueNodeMigrationEditor
{
    [MenuItem("Tools/Dialogue System/Migrate All Dialogue Nodes")]
    public static void MigrateAllNodes()
    {
        DialogueNodeMigrationTool tool = new DialogueNodeMigrationTool();
        tool.MigrateAllDialogueNodes();
    }
    
    [MenuItem("Tools/Dialogue System/Create Migration Tool")]
    public static void CreateMigrationTool()
    {
        GameObject go = new GameObject("DialogueNodeMigrationTool");
        go.AddComponent<DialogueNodeMigrationTool>();
        Selection.activeGameObject = go;
    }
    
    [MenuItem("Tools/Dialogue System/Batch Set Speaker")]
    public static void BatchSetSpeaker()
    {
        string speakerName = EditorUtility.SaveFilePanel("Set Speaker Name", "", "Default Speaker", "");
        if (!string.IsNullOrEmpty(speakerName))
        {
            DialogueNodeMigrationTool tool = new DialogueNodeMigrationTool();
            tool.BatchSetSpeaker(speakerName);
        }
    }
    
    [MenuItem("Tools/Dialogue System/Validate All Dialogue Nodes")]
    public static void ValidateAllNodes()
    {
        string[] guids = AssetDatabase.FindAssets("t:DialogueNode");
        int totalCount = 0;
        int validCount = 0;
        int invalidCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            DialogueNode node = AssetDatabase.LoadAssetAtPath<DialogueNode>(path);
            
            if (node != null)
            {
                totalCount++;
                if (node.IsValid())
                {
                    validCount++;
                }
                else
                {
                    invalidCount++;
                    Debug.LogWarning($"Invalid dialogue node: {node.name}");
                }
            }
        }
        
        Debug.Log($"Dialogue node validation completed: Total {totalCount}, Valid {validCount}, Invalid {invalidCount}");
        
        if (invalidCount > 0)
        {
            EditorUtility.DisplayDialog("Validation Result", 
                $"Found {invalidCount} invalid dialogue nodes, please check console for details.", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Validation Result", "All dialogue nodes are valid!", "OK");
        }
    }
}
#endif 