using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public Image itemImage;
    public TextMeshProUGUI shopItemName;
    public TextMeshProUGUI des;
    public TextMeshProUGUI PriceText;

    [Header("ItemInfo")]
    public string itemName;
    public int quantity = 1;
    public Sprite sprite;
    public string itemDescription;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemImage.sprite = sprite;
        shopItemName.text = itemName;
        des.text = itemDescription;
        PriceText.text = "$" + ShopController.instance.shopItems[2, ItemID].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
