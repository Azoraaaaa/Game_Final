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
    public int[,] shopItems = new int[7,7];
    public float coins;
    public TextMeshProUGUI CoinsText;

    public static ShopController instance;
    private void Awake()
    {
        instance = this;

        //ID
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;
        shopItems[1, 6] = 6;

        //Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;
        shopItems[2, 5] = 50;
        shopItems[2, 6] = 60;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CoinsText.text = coins.ToString();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public void ToggleShopScreen()
    {
        if (UIController.instance.ShopScreen.activeInHierarchy)
        {
            UIController.instance.ShopScreen.SetActive(false); // Hide the bag screen
            UIController.instance.SetHUDVisibility(true);
            //UIController.instance.HealthTextInBag.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else
        {
            UIController.instance.ShopScreen.SetActive(true); // Show the bag screen
            UIController.instance.SetHUDVisibility(false);
            //UIController.instance.HealthTextInBag.gameObject.SetActive(true);
            //UIController.instance.HealthTextInBag.text = "HEALTH: " + PlayerHealthController.instance.currentHealth + "/" + PlayerHealthController.instance.maxHealth;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f; //freeze the screen
        }
    }
    */
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo info = ButtonRef.GetComponent<ButtonInfo>();
        int price = shopItems[2, info.ItemID];

        if (coins >= price)
        {
            coins -= price;
            CoinsText.text = coins.ToString();

            int leftOver = InventoryController.instance.AddItem(info.itemName,info.quantity,info.sprite,info.itemDescription);
        }
        else
        {
            //cannot buy
        }
    }
}
