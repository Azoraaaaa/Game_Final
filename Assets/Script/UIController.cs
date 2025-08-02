using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UIController instance;

    public float coins;
    public GameObject BagScreen;
    public GameObject StoryScreen;
    public GameObject ShopScreen;

    public void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Find();

        //SetHUDVisibility(true);
        BagScreen.SetActive(false);
        StoryScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryController.instance.ToggleBagScreen();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StoryManager.instance.ToggleStoryScreen();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(3);
        }

    }
    public void SaveCoins()
    {
        PlayerPrefs.SetFloat("coinNum", coins);
    }
    public void CloseShop()
    {
        ShopScreen.SetActive(false);
    }
    public void Find()
    {
        GameObject newBag = GameObject.FindWithTag("BagScreen");
        if (newBag != null)
        {
            BagScreen = newBag;
            BagScreen.SetActive(false);
        }

        GameObject newStory = GameObject.FindWithTag("StoryScreen");
        if (newStory != null)
        {
            StoryScreen = newStory;
            StoryScreen.SetActive(false);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedFind());
    }

    IEnumerator DelayedFind()
    {
        yield return null; // 等待一帧，确保新场景加载完成
        Find();
    }
}
