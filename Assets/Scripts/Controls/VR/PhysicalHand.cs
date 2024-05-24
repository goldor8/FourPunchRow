using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class PhysicalHand : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    private Rigidbody rb;
    
    [SerializeField] private bool hideRealHandWhenClose = true;
    [SerializeField] private float hideRealHandDistance = 0.1f;
    
    [SerializeField] private float handMovementSpeed = 1000f;
    [SerializeField] private float MaxDistanceFactor = 1.5f;
    [SerializeField] private float handRotationSpeed = 1000f;
    [SerializeField] private float MaxRotationFactor = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 100;
    }
    
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        return a * Quaternion.Inverse(b);
    }
    
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    private void FixedUpdate()
    {
        Vector3 positionDifference = handTransform.position - transform.position;
        positionDifference = Vector3.ClampMagnitude(positionDifference, MaxDistanceFactor);
        
        rb.velocity = positionDifference * (handMovementSpeed * Time.fixedDeltaTime);
        
        //does not work because of angle sign in the difference
        Quaternion difference = ShortestRotation(handTransform.rotation, transform.rotation);
        // Quaternion aQuaternion = Quaternion.identity * Quaternion.Inverse(handTransform.rotation);
        // Quaternion bQuaternion = Quaternion.identity * Quaternion.Inverse(transform.rotation);
        // Quaternion difference = bQuaternion * Quaternion.Inverse(aQuaternion);
        difference.ToAngleAxis(out float angle, out Vector3 axis);
        angle = Mathf.Clamp(angle, -MaxRotationFactor, MaxRotationFactor);
        // Debug.Log(axis);
        rb.angularVelocity = axis * (angle * handRotationSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime);
        
        

        //here is a workaround
        //rb.angularVelocity = handRotationSpeed * (handTransform.rotation * Quaternion.Inverse(transform.rotation)).eulerAngles / Time.fixedDeltaTime;
        
        
        if (hideRealHandWhenClose)
        {
            if(positionDifference.magnitude < hideRealHandDistance)
            {
                handTransform.gameObject.SetActive(false);
            }
            else
            {
                handTransform.gameObject.SetActive(true);
            }
        }
    }
}
