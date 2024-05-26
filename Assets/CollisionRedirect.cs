using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionRedirect : MonoBehaviour
{
    public UnityEvent<Collision> onCollisionEnterEvent;

    private void OnCollisionEnter(Collision other)
    {
        onCollisionEnterEvent.Invoke(other);
    }
}
