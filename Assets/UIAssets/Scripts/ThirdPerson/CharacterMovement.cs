using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Vector3 CurrentInput { get; private set; }
    public float MaxWalkSpeed = 5;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + CurrentInput * MaxWalkSpeed * Time.fixedDeltaTime);
        
        if (CurrentInput.sqrMagnitude > 0)
        {
            // transform.rotation = Quaternion.LookRotation(CurrentInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurrentInput), 10 * Time.fixedDeltaTime);
        }
    }

    public void SetMovementInput(Vector3 input)
    {
        CurrentInput = Vector3.ClampMagnitude(input, 1);
    }
}
