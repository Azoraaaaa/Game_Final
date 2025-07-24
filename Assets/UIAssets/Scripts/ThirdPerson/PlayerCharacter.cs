using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    [SerializeField]
    private Photographer _photographer;

    [SerializeField] private Transform _followingTarget;

    private bool _inputEnabled;
    public void SetInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;
        if (enabled == false)
        {
            _characterMovement.SetMovementInput(Vector3.zero);
        }
    }

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        
        _photographer.InitCamera(_followingTarget);

        SetInputEnabled(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovementInput();
    }

    private void UpdateMovementInput()
    {
        if (!_inputEnabled) return;
        Quaternion rot = Quaternion.Euler(0,_photographer.Yaw,0);
        
        _characterMovement.SetMovementInput(rot * Vector3.forward * Input.GetAxis("Vertical") + 
                                            rot * Vector3.right * Input.GetAxis("Horizontal"));
    }
}
