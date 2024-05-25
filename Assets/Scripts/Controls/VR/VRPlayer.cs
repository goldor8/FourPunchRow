using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : Player
{
    [SerializeField] private GameObject[] HandInteractionGameObjects;
    [SerializeField] private XRInteractor xrInteractor;

    [SerializeField] private GameObject[] BoxGloveGameObjects;

    [SerializeField] private PlayerMode playerMode;
    
    public enum PlayerMode
    {
        Interacting,
        Boxing,
    }
    
    public void SetHandInteractionEnabled(bool value)
    {
        xrInteractor.enabled = value;
        foreach (GameObject handInteractionGameObject in HandInteractionGameObjects)
        {
            handInteractionGameObject.SetActive(value);
        }
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
                break;
            case PlayerMode.Boxing:
                SetHandInteractionEnabled(false);
                SetBoxGloveEnabled(true);
                break;
        }
    }

    private void OnValidate()
    {
        SetPlayerMode(playerMode);
    }
}
