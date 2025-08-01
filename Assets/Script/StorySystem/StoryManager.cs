using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    private void Awake()
    {
        instance = this;
    }
    
    [Header("Paper")]
    public GameObject Paper1;
    public GameObject Paper2;
    public GameObject Paper3;
    public GameObject Paper4;
    public GameObject Paper5;

    [Header("UIButton")]
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;
    public GameObject Button5;

    [Header("Bool")]
    public bool isCollected1 = false;
    public bool isCollected2 = false;
    public bool isCollected3 = false;
    public bool isCollected4 = false;
    public bool isCollected5 = false;

    [Header("Ending")]
    public GameObject goodEnding;
    public GameObject badEnding;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        Button4.SetActive(false);
        Button5.SetActive(false);

        Paper1.SetActive(false);
        Paper2.SetActive(false);
        Paper3.SetActive(false);
        Paper4.SetActive(false);
        Paper5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleStoryScreen()
    {
        if (UIController.instance.StoryScreen.activeInHierarchy)
        {
            UIController.instance.StoryScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else
        {
            UIController.instance.StoryScreen.SetActive(true);

            CheckCollect();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
    public void CheckCollect()
    {
        if (isCollected1)
        {
           Button1.SetActive(true);
        }
        if (isCollected2)
        {
            Button2.SetActive(true);
        }
        if (isCollected3)
        {
            Button3.SetActive(true);
        }
        if (isCollected4)
        {
            Button4.SetActive(true);
        }
        if (isCollected5)
        {
            Button5.SetActive(true);
        }
    }
    public void OpenPaper1()
    {
        Paper1.SetActive(true);
    }
    public void ClosePaper1()
    {
        Paper1.SetActive(false);
    }
    public void OpenPaper2()
    {
        Paper2.SetActive(true);
    }
    public void ClosePaper2()
    {
        Paper2.SetActive(false);
    }
    public void OpenPaper3()
    {
        Paper3.SetActive(true);
    }
    public void ClosePaper3()
    {
        Paper3.SetActive(false);
    }
    public void OpenPaper4()
    {
        Paper4.SetActive(true);
    }
    public void ClosePaper4()
    {
        Paper4.SetActive(false);
    }
    public void OpenPaper5()
    {
        Paper5.SetActive(true);
    }
    public void ClosePaper5()
    {
        Paper5.SetActive(false);
    }
    public void CheckEnd()
    {
        if(isCollected1 && isCollected2 && isCollected3 && isCollected4 && isCollected5)
        { 
            goodEnding.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            badEnding.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
