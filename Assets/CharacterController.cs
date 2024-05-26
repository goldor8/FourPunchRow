using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    
    [SerializeField]
    private Rigidbody playerRigidbody;
    
    [SerializeField] Transform playerHead;
    
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    
    
    public void Move(Vector2 horizontalMovement, float yRotation)
    {
        Vector3 movement = new Vector3(horizontalMovement.x, 0, horizontalMovement.y);
        Vector3 rotation = new Vector3(0, yRotation, 0);
        
        movement = playerHead.transform.TransformDirection(movement);
        if (horizontalMovement.magnitude > 1)
            movement = movement.normalized;
        
        playerRigidbody.AddForce(movement * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        playerRigidbody.AddTorque(rotation * rotationSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}