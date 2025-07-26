using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public GameObject dialogueBox;
    public Transform choiceButtonsContainer;
    public GameObject choiceButtonPrefab;
    
    [Header("Linear Dialogue")]
    public Button continueButton; // 用于线性对话的继续按钮

    private DialogueNode currentNode;
    private List<GameObject> activeChoiceButtons = new List<GameObject>();
    private bool isTyping = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            dialogueBox.SetActive(false);
            choiceButtonsContainer.gameObject.SetActive(false);
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(false);
                continueButton.onClick.AddListener(OnContinueButtonClicked);
            }
        }
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (TimelineManager.Instance != null)
        {
            TimelineManager.Instance.StartDialogue();
        }
        
        dialogueBox.SetActive(true);
        ShowNode(startNode);
    }

    private void ShowNode(DialogueNode node)
    {
        currentNode = node;

        // Clear previous choices
        foreach (GameObject button in activeChoiceButtons)
        {
            Destroy(button);
        }
        activeChoiceButtons.Clear();
        choiceButtonsContainer.gameObject.SetActive(false);
        
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(node.sentence));

        speakerNameText.text = node.speakerName;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        isTyping = false;

        // Finished typing, show choices or continue button
        DisplayChoices();
    }

    private void DisplayChoices()
    {
        if (currentNode.choices == null || currentNode.choices.Count == 0)
        {
            // 线性对话：显示继续按钮
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true);
            }
        }
        else
        {
            // 分支对话：创建选项按钮
            foreach (PlayerChoice choice in currentNode.choices)
            {
                GameObject buttonGO = Instantiate(choiceButtonPrefab, choiceButtonsContainer);
                activeChoiceButtons.Add(buttonGO);

                TMP_Text buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
                buttonText.text = choice.choiceText;

                Button button = buttonGO.GetComponent<Button>();
                button.onClick.AddListener(() => OnChoiceSelected(choice));
            }
            choiceButtonsContainer.gameObject.SetActive(true);
        }
    }

    private void OnContinueButtonClicked()
    {
        // 线性对话结束
        EndDialogue();
    }

    private void OnChoiceSelected(PlayerChoice choice)
    {
        // Trigger any events attached to the choice
        choice.onChoiceSelected?.Invoke();

        // If there's a next node, go to it. Otherwise, end the dialogue.
        if (choice.nextNode != null)
        {
            ShowNode(choice.nextNode);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        choiceButtonsContainer.gameObject.SetActive(false);
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }

        foreach (GameObject button in activeChoiceButtons)
        {
            Destroy(button);
        }
        activeChoiceButtons.Clear();

        if (TimelineManager.Instance != null)
        {
            TimelineManager.Instance.EndDialogue();
        }
    }

    // 用于外部调用的继续对话方法（比如点击屏幕继续）
    public void ContinueDialogue()
    {
        if (isTyping)
        {
            // 如果正在打字，直接显示完整句子
            StopAllCoroutines();
            dialogueText.text = currentNode.sentence;
            isTyping = false;
            DisplayChoices();
        }
        else if (continueButton != null && continueButton.gameObject.activeInHierarchy)
        {
            // 如果继续按钮可见，点击它
            OnContinueButtonClicked();
        }
    }
} 