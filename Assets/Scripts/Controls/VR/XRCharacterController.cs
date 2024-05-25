using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(XRInteractor))]
[RequireComponent(typeof(Rigidbody))]
public class XRCharacterController : MonoBehaviour
{
    public InputActionProperty rightThumbstickAction;
    public InputActionProperty leftThumbstickAction;
    
    [SerializeField]
    private Rigidbody playerRigidbody;
    
    [SerializeField] Camera playerHead;
    
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    
    private void Start()
    {
    }

    void FixedUpdate()
    {
            ApplyGMovement();
    }

    private void ApplyGMovement()
    {
        Vector2 rightThumbstick = rightThumbstickAction.action.ReadValue<Vector2>();
        Vector2 leftThumbstick = leftThumbstickAction.action.ReadValue<Vector2>();
        
        Vector3 movement = new Vector3(rightThumbstick.x, 0, rightThumbstick.y);
        Vector3 rotation = new Vector3(0, leftThumbstick.x, 0);
        
        movement = playerHead.transform.TransformDirection(movement);
        movement.y = 0;
        movement.Normalize();
        
        playerRigidbody.AddForce(movement * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        playerRigidbody.AddTorque(rotation * rotationSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
