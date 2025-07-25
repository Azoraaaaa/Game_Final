using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public bool IsUIMode { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 默认设置为游戏模式
        SetGameMode();
    }

    public void SetUIMode()
    {
        IsUIMode = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetGameMode()
    {
        IsUIMode = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ESC键在两种模式之间切换
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsUIMode)
            {
                SetGameMode();
            }
            else
            {
                SetUIMode();
            }
        }
    }
} 