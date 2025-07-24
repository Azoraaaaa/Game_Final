using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Test : MonoBehaviour
{
    public SequenceEventExecutor executor;
    public PlayerCharacter playerCharacter;

    private void Start()
    {
        executor.Init(OnFinishedEvent);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            playerCharacter.SetInputEnabled(false);
            executor.Execute();
        }
    }

    void OnFinishedEvent(bool success)
    {
        playerCharacter.SetInputEnabled(true);
    }
}
