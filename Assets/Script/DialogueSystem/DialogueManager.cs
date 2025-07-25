using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    private Queue<string> sentences;
    private bool isTyping = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueData data)
    {
        // 通知TimelineManager开始对话
        if (TimelineManager.Instance != null)
        {
            TimelineManager.Instance.StartDialogue();
        }

        dialogueBox.SetActive(true);
        speakerNameText.text = data.speakerName;

        sentences.Clear();

        foreach (string sentence in data.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // 如果正在打字，直接显示完整句子
        if (isTyping)
        {
            StopAllCoroutines();
            if (sentences.Count > 0)
            {
                dialogueText.text = sentences.Peek();
            }
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // 添加一个小延迟使打字效果更明显
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);

        // 通知TimelineManager对话结束
        if (TimelineManager.Instance != null)
        {
            TimelineManager.Instance.EndDialogue();
        }
    }

    // 用于Timeline或其他系统强制结束对话
    public void ForceEndDialogue()
    {
        StopAllCoroutines();
        sentences.Clear();
        EndDialogue();
    }
} 