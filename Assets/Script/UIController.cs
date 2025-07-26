using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject ShopScreen;
    //public Text HealthTextInBag;

    //public Text speedBoostText;

    //public Text bossHint;
    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //SetHUDVisibility(true);
        BagScreen.SetActive(false);
        ShopScreen.SetActive(false);
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
}
