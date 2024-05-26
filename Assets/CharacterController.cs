using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    
    [SerializeField]
    private Rigidbody playerRigidbody;
    
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    
    
    public void Move(Vector2 horizontalMovement, float yRotation)
    {
        Vector3 movement = new Vector3(horizontalMovement.x, 0, horizontalMovement.y);
        Vector3 rotation = new Vector3(0, yRotation, 0);
        
        movement = playerRigidbody.transform.TransformDirection(movement);
        if (horizontalMovement.magnitude > 1)
            movement = movement.normalized;
        
        playerRigidbody.velocity = movement * movementSpeed * Time.fixedDeltaTime;
        Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + playerRigidbody.velocity, Color.green);
        playerRigidbody.angularVelocity = rotation * rotationSpeed * Time.fixedDeltaTime;
    }
}