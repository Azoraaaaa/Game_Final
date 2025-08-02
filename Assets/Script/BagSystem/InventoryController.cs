using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;
    public ItemSlot[] itemSlot;

    public ItemSO[] itemSOs;

    public Text Use;
    public Text Drop;
    public Image LeftClick;
    public Image RightClick;
    public TextMeshProUGUI CoinsText;

    public void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleBagScreen()
    {
        if (UIController.instance.BagScreen.activeInHierarchy)
        {
            UIController.instance.BagScreen.SetActive(false); // Hide the bag screen
            Use.gameObject.SetActive(false);
            Drop.gameObject.SetActive(false);
            LeftClick.gameObject.SetActive(false);
            RightClick.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else
        {
            UIController.instance.BagScreen.SetActive(true); // Show the bag screen
            Use.gameObject.SetActive(true);
            Drop.gameObject.SetActive(true);
            LeftClick.gameObject.SetActive(true);
            RightClick.gameObject.SetActive(true);
            updateCoins();

            DeselectAllSlots();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; //freeze the screen
        }
    }
    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
            }
        }
        return false;
    }

    public int AddItem(string itemName, int quantity, Sprite sprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, sprite, itemDescription);
                if (leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, sprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
    public void RemoveItemByName(string itemName)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].quantity = 0; // 清空物品数量
                itemSlot[i].isFull = false; // 标记为未满
                itemSlot[i].itemName = ""; // 清空物品名称

                // 清空 UI
                itemSlot[i].itemImage.sprite = itemSlot[i].emptySprite;
                itemSlot[i].quantityText.enabled = false;
                itemSlot[i].itemDescriptionNameText.text = "";
                itemSlot[i].itemDescriptionText.text = "";
                itemSlot[i].itemDescriptionImage.sprite = itemSlot[i].emptySprite;

                Debug.Log($"Removed item: {itemName}");
                break; // 找到第一个匹配的物品后退出
            }
        }
    }
    public void updateCoins()
    {
        CoinsText.text = UIController.instance.coins.ToString();
    }

}
