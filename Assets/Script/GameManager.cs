using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour

{
    public static GameManager instance;

    public GameObject player;

    public bool isTalking = false;

    public HashSet<string> discoveredLocations = new HashSet<string>();
    public bool isNearTeleporter = false; // 玩家是否在传送点附近

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryController.instance.ToggleBagScreen();

            //PlaySound(openSound);
        }

    }

    public void LoadScene0()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
