using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class VRPlayer : Player
{
    [SerializeField] private GameObject[] HandInteractionGameObjects;
    [SerializeField] private XRInteractor xrInteractor;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private Transform interactingPos;
    [SerializeField] private Transform boxingPos;

    [SerializeField] private GameObject[] BoxGloveGameObjects;

    [SerializeField] private PlayerMode playerMode;
    [SerializeField] private Transform headTransform;
    [SerializeField] private Transform bodyTransform;
    public UnityEvent<int> OnDamageValueChanged;

    private int damages;
    
    public enum PlayerMode
    {
        Interacting,
        Boxing,
    }

    protected virtual void Start()
    {
        GameManager.Singleton.SetPlayer(this);
    }

    public void SetHandInteractionEnabled(bool value)
    {
        if (xrInteractor != null)
        {
            xrInteractor.enabled = value;
        }
        foreach (GameObject handInteractionGameObject in HandInteractionGameObjects)
        {
            handInteractionGameObject.SetActive(value);
        }
    }

    public void SetCharacterControllerEnabled(bool value)
    {
        characterController.enabled = value;
        characterController.GetRigidBody().isKinematic = !value;
    }

    public void SetBoxGloveEnabled(bool value)
    {
        foreach (GameObject boxGloveGameObject in BoxGloveGameObjects)
        {
            boxGloveGameObject.SetActive(value);
        }
    }

    public void SetPlayerMode(PlayerMode mode)
    {
        playerMode = mode;

        switch (mode)
        {
            case PlayerMode.Interacting:
                SetBoxGloveEnabled(false);
                SetHandInteractionEnabled(true);
                SetCharacterControllerEnabled(false);
                transform.position = interactingPos.position;
                transform.rotation = interactingPos.rotation;
                break;
            case PlayerMode.Boxing:
                SetHandInteractionEnabled(false);
                SetBoxGloveEnabled(true);
                SetCharacterControllerEnabled(true);
                transform.position = boxingPos.position;
                transform.rotation = boxingPos.rotation;
                break;
        }
    }

    private void OnValidate()
    {
        SetPlayerMode(playerMode);
    }

    public Transform GetHeadTransform()
    {
        return headTransform;
    }

    public Transform GetBodyTransform()
    {
        return bodyTransform;
    }

    public void Damage(int value)
    {
        damages += value;
        OnDamageValueChanged.Invoke(damages);
    }

    public int GetDamages()
    {
        return damages;
    }
}
