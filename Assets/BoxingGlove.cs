using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class BoxingGlove : MonoBehaviour
{
    [SerializeField] private AICharacterController ai;
    [SerializeField] private bool rightHand;

    [SerializeField] private MeshRenderer GraphicGlove;
    [SerializeField] private Color counteringColor;
    [SerializeField] private Color baseColor;

    [SerializeField] private InputActionProperty counterAction;
    [SerializeField] private float activeTime;
    [SerializeField] private float cooldownTime;
    private float timeBeforeAvailable;
    private bool countering;

    private void Update()
    {
        if (counterAction != null && counterAction.action.ReadValue<float>() > 0.8f)
        {
            Counter();
        }
    }

    private void Counter()
    {
        if (timeBeforeAvailable < Time.time)
        {
            countering = true;
            GraphicGlove.material.color = counteringColor;
            Invoke(nameof(DisableCounter), activeTime);
            timeBeforeAvailable = Time.time + activeTime + cooldownTime;
        }
    }

    private void DisableCounter()
    {
        countering = false;
        GraphicGlove.material.color = baseColor;
    }

    private void OnCollisionEnter(Collision other)
    {
        PlayerDamageable player = other.collider.gameObject.GetComponent<PlayerDamageable>();
        if(player != null)
            player.MarkHit();

        Debug.Log(other.gameObject);
        
        BoxingGlove glove = other.gameObject.GetComponent<BoxingGlove>();
        if(glove != null && countering)
            glove.CancelHit();
    }

    private void CancelHit()
    {
        Debug.Log("asking cancel");
        if (ai != null)
        {
            ai.CancelPunch(rightHand);
        }
    }
}
