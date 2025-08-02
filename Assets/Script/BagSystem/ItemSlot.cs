using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite sprite;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    [SerializeField]
    public int maxNumberOfItems;

    [SerializeField]
    public Text quantityText;

    [SerializeField]
    public Image itemImage;

    public GameObject selectedShader;
    public bool thisItemSelected;

    public Image itemDescriptionImage;
    public TextMeshProUGUI itemDescriptionNameText;
    public TextMeshProUGUI itemDescriptionText;


    public int AddItem(string itemName, int quantity, Sprite sprite, string itemDescription)
    {
        if (isFull)
        {
            return quantity;
        }

        if (this.quantity == 0)
        {
            this.itemName = itemName;
            this.sprite = sprite;
            this.itemDescription = itemDescription;

            itemImage.enabled = true;
            itemImage.sprite = sprite;
        }

        this.quantity += quantity;
        Debug.Log($"Quantity: {this.quantity}");

        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;

            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            bool usable = InventoryController.instance.UseItem(itemName);
            if (usable)
            {
                Debug.Log("Item used");
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                {
                    EmptySLot();
                }
            }
        }
        else
        {
            InventoryController.instance.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = sprite;
            if (itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
        }

    }
    private void EmptySLot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        itemImage.enabled = false;
        itemName = "";
        itemDescription = "";
        sprite = emptySprite;
        isFull = false;

        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }

    public void OnRightClick()
    {
        GameObject itemToDrop = new GameObject(itemName);
        Item newItem = itemToDrop.AddComponent<Item>();
        newItem.quantity = 1;
        newItem.itemName = itemName;
        newItem.sprite = sprite;
        newItem.itemDescription = itemDescription;
        newItem.pickupDelay = 1f;

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        BoxCollider collider = itemToDrop.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(0, 0.3f, 0.2f);
        itemToDrop.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        this.quantity -= 1;
        quantityText.text = this.quantity.ToString();
        if (this.quantity <= 0)
        {
            EmptySLot();
        }

        UIController.instance.coins = UIController.instance.coins + 6;
        UIController.instance.SaveCoins();
        InventoryController.instance.updateCoins();
    }
}
