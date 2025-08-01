using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class ShopController : MonoBehaviour
{
    public int[,] shopItems = new int[5,5];
    public TextMeshProUGUI CoinsText;
    public int price1;
    public int price2;
    public int price3;
    public int price4;

    public static ShopController instance;
    private void Awake()
    {
        instance = this;

        //ID
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        //Price
        shopItems[2, 1] = price1;
        shopItems[2, 2] = price2;
        shopItems[2, 3] = price3;
        shopItems[2, 4] = price4;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinsText.text = UIController.instance.coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo info = ButtonRef.GetComponent<ButtonInfo>();
        int price = shopItems[2, info.ItemID];

        if (UIController.instance.coins >= price)
        {
            UIController.instance.coins -= price;
            CoinsText.text = UIController.instance.coins.ToString();
            UIController.instance.SaveCoins();

            int leftOver = InventoryController.instance.AddItem(info.itemName,info.quantity,info.sprite,info.itemDescription);
        }
        else
        {
            //cannot buy
        }
    }
}
