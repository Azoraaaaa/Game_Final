using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for dialogue nodes
/// Provides better UI for managing multi-speaker dialogue nodes
/// </summary>
[CustomEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : Editor
{
    private DialogueNode dialogueNode;
    private bool showSentences = true;
    private bool showChoices = true;
    private bool showValidation = true;
    
    private void OnEnable()
    {
        dialogueNode = (DialogueNode)target;
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        // Default speaker settings
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Default Speaker Settings", EditorStyles.boldLabel);
        SerializedProperty defaultSpeakerProp = serializedObject.FindProperty("defaultSpeakerName");
        EditorGUILayout.PropertyField(defaultSpeakerProp, new GUIContent("Default Speaker Name"));
        
        // Dialogue content settings
        EditorGUILayout.Space();
        showSentences = EditorGUILayout.Foldout(showSentences, "Dialogue Content Settings", true);
        if (showSentences)
        {
            EditorGUI.indentLevel++;
            
            SerializedProperty sentencesProp = serializedObject.FindProperty("sentences");
            SerializedProperty speakerNamesProp = serializedObject.FindProperty("speakerNames");
            
            // Synchronize array sizes
            if (sentencesProp.arraySize != speakerNamesProp.arraySize)
            {
                speakerNamesProp.arraySize = sentencesProp.arraySize;
            }
            
            // Display array size control
            EditorGUILayout.BeginHorizontal();
            int newSize = EditorGUILayout.IntField("Number of Sentences", sentencesProp.arraySize);
            if (newSize != sentencesProp.arraySize)
            {
                sentencesProp.arraySize = newSize;
                speakerNamesProp.arraySize = newSize;
            }
            
            if (GUILayout.Button("Add Sentence", GUILayout.Width(80)))
            {
                sentencesProp.arraySize++;
                speakerNamesProp.arraySize++;
            }
            EditorGUILayout.EndHorizontal();
            
            // Display each sentence and its corresponding speaker
            for (int i = 0; i < sentencesProp.arraySize; i++)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField($"Sentence {i + 1}", EditorStyles.boldLabel);
                
                // Speaker settings
                SerializedProperty speakerProp = speakerNamesProp.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Speaker:", GUILayout.Width(60));
                speakerProp.stringValue = EditorGUILayout.TextField(speakerProp.stringValue);
                if (GUILayout.Button("Use Default", GUILayout.Width(60)))
                {
                    speakerProp.stringValue = "";
                }
                EditorGUILayout.EndHorizontal();
                
                // Sentence content
                SerializedProperty sentenceProp = sentencesProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(sentenceProp, new GUIContent("Sentence Content"), GUILayout.Height(60));
                
                // Delete button
                if (GUILayout.Button("Delete This Sentence", GUILayout.Width(120)))
                {
                    sentencesProp.DeleteArrayElementAtIndex(i);
                    speakerNamesProp.DeleteArrayElementAtIndex(i);
                    break;
                }
            }
            
            EditorGUI.indentLevel--;
        }
        
        // Player choice settings
        EditorGUILayout.Space();
        showChoices = EditorGUILayout.Foldout(showChoices, "Player Choice Settings", true);
        if (showChoices)
        {
            EditorGUI.indentLevel++;
            SerializedProperty choicesProp = serializedObject.FindProperty("choices");
            EditorGUILayout.PropertyField(choicesProp, true);
            EditorGUI.indentLevel--;
        }
        
        // Validation information
        EditorGUILayout.Space();
        showValidation = EditorGUILayout.Foldout(showValidation, "Validation Information", true);
        if (showValidation)
        {
            EditorGUI.indentLevel++;
            
            if (!dialogueNode.IsValid())
            {
                EditorGUILayout.HelpBox("Warning: This dialogue node has no valid sentence content!", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("Dialogue node configuration is valid", MessageType.Info);
            }
            
            // Display statistics
            int validSentences = 0;
            int uniqueSpeakers = 0;
            System.Collections.Generic.HashSet<string> speakers = new System.Collections.Generic.HashSet<string>();
            
            if (dialogueNode.sentences != null)
            {
                for (int i = 0; i < dialogueNode.sentences.Length; i++)
                {
                    if (!string.IsNullOrEmpty(dialogueNode.sentences[i]))
                    {
                        validSentences++;
                        string speaker = dialogueNode.GetSpeakerName(i);
                        if (!string.IsNullOrEmpty(speaker))
                        {
                            speakers.Add(speaker);
                        }
                    }
                }
            }
            
            uniqueSpeakers = speakers.Count;
            
            EditorGUILayout.LabelField($"Number of Valid Sentences: {validSentences}");
            EditorGUILayout.LabelField($"Number of Participating Characters: {uniqueSpeakers}");
            
            if (uniqueSpeakers > 0)
            {
                EditorGUILayout.LabelField("Participating Characters:");
                foreach (string speaker in speakers)
                {
                    EditorGUILayout.LabelField($"  - {speaker}");
                }
            }
            
            EditorGUI.indentLevel--;
        }
        
        // Apply changes
        serializedObject.ApplyModifiedProperties();
        
        // Preview button
        EditorGUILayout.Space();
        if (GUILayout.Button("Preview Dialogue"))
        {
            PreviewDialogue();
        }
    }
    
    /// <summary>
    /// Preview dialogue content
    /// </summary>
    private void PreviewDialogue()
    {
        if (!dialogueNode.IsValid())
        {
            EditorUtility.DisplayDialog("Preview Failed", "This dialogue node has no valid sentence content!", "OK");
            return;
        }
        
        string preview = "Dialogue Preview:\n\n";
        
        for (int i = 0; i < dialogueNode.sentences.Length; i++)
        {
            if (!string.IsNullOrEmpty(dialogueNode.sentences[i]))
            {
                string speaker = dialogueNode.GetSpeakerName(i);
                preview += $"{speaker}: {dialogueNode.sentences[i]}\n\n";
            }
        }
        
        if (dialogueNode.choices != null && dialogueNode.choices.Count > 0)
        {
            preview += "Choice Options:\n";
            for (int i = 0; i < dialogueNode.choices.Count; i++)
            {
                preview += $"{i + 1}. {dialogueNode.choices[i].choiceText}\n";
            }
        }
        else
        {
            preview += "No choice options";
        }
        
        EditorUtility.DisplayDialog("Dialogue Preview", preview, "OK");
    }
} 