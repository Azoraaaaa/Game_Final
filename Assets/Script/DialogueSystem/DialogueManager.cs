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
    public Button continueButton; // 新增：自定义继续按钮

    private DialogueNode currentNode;
    private List<GameObject> activeChoiceButtons = new List<GameObject>();

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
            if (continueButton != null) continueButton.gameObject.SetActive(false);
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

        // 清理旧按钮
        foreach (GameObject button in activeChoiceButtons)
            Destroy(button);
        activeChoiceButtons.Clear();
        choiceButtonsContainer.gameObject.SetActive(false);
        if (continueButton != null) continueButton.gameObject.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(node.sentence));
        speakerNameText.text = node.speakerName;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        DisplayChoicesOrContinue();
    }

    private void DisplayChoicesOrContinue()
    {
        if (currentNode.choices == null || currentNode.choices.Count == 0)
        {
            // 没有分支，显示“继续”按钮
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(true);
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(EndDialogue);
            }
        }
        else
        {
            // 有分支，显示选项按钮
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

    private void OnChoiceSelected(PlayerChoice choice)
    {
        // 事件分发
        switch (choice.eventType)
        {
            case DialogueChoiceEventType.OpenShop:
                UIManager.instance.OpenShopPanel();
                break;
            case DialogueChoiceEventType.OpenBag:
                UIManager.instance.OpenBagPanel();
                break;
            case DialogueChoiceEventType.CloseShop:
                UIManager.instance.CloseShopPanel();
                break;
            case DialogueChoiceEventType.CloseBag:
                UIManager.instance.CloseBagPanel();
                break;
            
            case DialogueChoiceEventType.StartQuest:
                if (!string.IsNullOrEmpty(choice.stringParameter))
                {
                    QuestManager.Instance.StartQuest(choice.stringParameter);
                }
                break;
            // 可扩展更多事件
            // case DialogueChoiceEventType.CloseShopAndEnd:
            //     UIManager.instance.CloseShopPanelAndEndDialogue();
            //     break;
            // case DialogueChoiceEventType.CloseBagAndEnd:
            //     UIManager.instance.CloseBagPanelAndEndDialogue();
            //     break;
        }

        // 跳转下一个节点或结束
        if (choice.nextNode != null)
            ShowNode(choice.nextNode);
        else
            EndDialogue();
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        choiceButtonsContainer.gameObject.SetActive(false);
        if (continueButton != null) continueButton.gameObject.SetActive(false);
        foreach (GameObject button in activeChoiceButtons)
            Destroy(button);
        activeChoiceButtons.Clear();
        if (TimelineManager.Instance != null)
        {
            TimelineManager.Instance.EndDialogue();
        }
    }
} 