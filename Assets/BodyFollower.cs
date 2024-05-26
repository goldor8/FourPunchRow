using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BodyFollower : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform head;
    [SerializeField] private float heightOffset;
    [SerializeField] private float maxDistanceFactor;
    [SerializeField] private float bodySpeed;
    

    private Vector3 lastBodyPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 posDifference = head.transform.position - transform.position;
        posDifference = Vector3.ClampMagnitude(posDifference, maxDistanceFactor);

        rb.velocity = posDifference * Time.fixedDeltaTime * bodySpeed;
        rb.transform.rotation = Quaternion.Euler(rb.transform.rotation.eulerAngles.x, head.rotation.eulerAngles.y, rb.transform.rotation.eulerAngles.z);
    }
}
