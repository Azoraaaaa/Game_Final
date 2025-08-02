using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointResetter : MonoBehaviour
{
    void Awake()
    {
        string key = SceneManager.GetActiveScene().name + "_cp";
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            Debug.Log("delete old checkpoint");
        }
    }
}
