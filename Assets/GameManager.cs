using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [SerializeField] private VRPlayer player;
    [SerializeField] private VRPlayer aiPlayer;

    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("A GameManager is already in the scene");
            return;
        }

        Singleton = this;
    }

    public void SetPlayer(VRPlayer player)
    {
        this.player = player;
    }

    public void SetAIPlayer(VRPlayer aiPlayer)
    {
        this.aiPlayer = aiPlayer;
    }

    public VRPlayer GetPlayer()
    {
        return player;
    }

    public VRPlayer GetAIPlayer()
    {
        return aiPlayer;
    }
    
    
}
