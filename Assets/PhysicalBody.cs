using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CapsuleCollider))]
public class PhysicalBody : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    [SerializeField] private Transform playerHead;

    [SerializeField] private float maxHeight;
    [SerializeField] private float minHeight;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

    }
    
    private void FixedUpdate()
    {
        capsuleCollider.height = Mathf.Clamp(playerHead.localPosition.y, minHeight, maxHeight);
        capsuleCollider.center = new Vector3(playerHead.localPosition.x, capsuleCollider.height / 2, playerHead.localPosition.z);
    }
}
