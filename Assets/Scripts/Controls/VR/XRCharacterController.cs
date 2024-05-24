using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(XRInteractor))]
[RequireComponent(typeof(Rigidbody))]
public class XRCharacterController : MonoBehaviour
{
    public InputActionProperty rightThumbstickAction;
    public InputActionProperty leftThumbstickAction;
    
    public InputActionProperty rightThumbstickClickAction;
    
    public Transform LeftHandTransform;
    public CollisionDetection leftHandGrabbing;
    public Transform RightHandTransform;
    public CollisionDetection rightHandGrabbing;
    
    [SerializeField]
    private Rigidbody playerRigidbody;
    [SerializeField]
    private XRInteractor interactor;
    
    private Vector3 prevLeftHandPosition;
    private Vector3 prevRightHandPosition;
    
    private bool isLeftHandGrabbing;
    private bool isRightHandGrabbing;
    
    [SerializeField] private bool zeroG = false;
    [SerializeField] Collider zeroGCollider;
    [SerializeField] Collider GCollider;
    
    [SerializeField] Camera playerHead;
    
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    
    private void Start()
    {
        
        rightThumbstickClickAction.action.performed += ctx => SwitchGravityMode();
        
        prevLeftHandPosition = LeftHandTransform.position;
        prevRightHandPosition = RightHandTransform.position;
    }

    void FixedUpdate()
    {
        if (zeroG)
        {
            ComputeAndApplyZeroGMovement();
        }
        else
        {
            ApplyGMovement();
        }
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

    void ComputeAndApplyZeroGMovement()
    {
        Vector3 currentLeftHandPosition = LeftHandTransform.position;
        Vector3 currentRightHandPosition = RightHandTransform.position;

        Vector3 leftHandVelocity = (currentLeftHandPosition - prevLeftHandPosition) / Time.fixedDeltaTime;
        Vector3 rightHandVelocity = (currentRightHandPosition - prevRightHandPosition) / Time.fixedDeltaTime;
        
        if(isLeftHandGrabbing && leftHandGrabbing.collidedObject != null && isRightHandGrabbing && leftHandGrabbing.collidedObject == rightHandGrabbing.collidedObject) // if both hands are grabbing the same object
        {
            //playerRigidbody.AddForce((leftHandVelocity + rightHandVelocity) / 2, ForceMode.VelocityChange);
        }
        else
        {
            if (isLeftHandGrabbing && leftHandGrabbing.collidedObject != null)
            {
                playerRigidbody.AddForce(-leftHandVelocity, ForceMode.VelocityChange);
            }

            if (isRightHandGrabbing && rightHandGrabbing.collidedObject != null)
            {
                playerRigidbody.AddForce(-rightHandVelocity, ForceMode.VelocityChange);
            }
        }

        prevLeftHandPosition = currentLeftHandPosition;
        prevRightHandPosition = currentRightHandPosition;
    }
    
    private void SwitchGravityMode()
    {
        SwitchGravityMode(zeroG);
    }
    
    private void SwitchGravityMode(bool affected)
    {
        playerRigidbody.useGravity = affected;
        zeroGCollider.enabled = !affected;
        GCollider.enabled = affected;
        zeroG = !affected;
    }

    private void OnValidate()
    {
        SwitchGravityMode(!zeroG);
    }
}
