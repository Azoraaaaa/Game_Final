using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public String cpName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
        {
            if(PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName)
            {
                PlayerController.instance.GetComponent<CharacterController>().enabled = false;
                PlayerController.instance.transform.position = transform.position;
                PlayerController.instance.transform.rotation = transform.rotation;
                Debug.Log("Player starting at " + cpName);
                PlayerController.instance.GetComponent<CharacterController>().enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName);
            Debug.Log("Player hit " + cpName);
        }
    }
}
