using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingGlove : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit");
    }
}
