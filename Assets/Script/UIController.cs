using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static UIController instance;
    /*
    public Slider healthSlider;
    public Text healthText;
    public Image healthImage;

    public Text AmmoText;
    public Image AmmoImage;

    public Slider energySlider;
    public Image energyImage;

    public Text hint;
    public Canvas questCanvas;
    public Canvas missionPointCanvas;

    public GameObject pauseScreen;
    */

    public GameObject BagScreen;
    public GameObject StoryScreen;

    //public Text HealthTextInBag;

    //public Text speedBoostText;

    //public Text bossHint;
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

    }
    public void SetHUDVisibility(bool isVisible)
    {
        /*
        healthSlider.gameObject.SetActive(isVisible);
        healthText.gameObject.SetActive(isVisible);
        healthImage.gameObject.SetActive(isVisible);
        AmmoText.gameObject.SetActive(isVisible);
        AmmoImage.gameObject.SetActive(isVisible);
        energySlider.gameObject.SetActive(isVisible);
        energyImage.gameObject.SetActive(isVisible);
        hint.gameObject.SetActive(isVisible);
        questCanvas.gameObject.SetActive(isVisible);
        missionPointCanvas.gameObject.SetActive(isVisible);
        */
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
