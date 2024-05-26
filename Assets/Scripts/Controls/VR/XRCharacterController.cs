using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRCharacterController : CharacterController
{
    public InputActionProperty rightThumbstickAction;
    public InputActionProperty leftThumbstickAction;
    
    private void Start()
    {
    }

    void FixedUpdate()
    {
        
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector2 rightThumbstick = rightThumbstickAction.action.ReadValue<Vector2>();
        Vector2 leftThumbstick = leftThumbstickAction.action.ReadValue<Vector2>();
        
        Move(rightThumbstick, leftThumbstick.x);
    }
}
