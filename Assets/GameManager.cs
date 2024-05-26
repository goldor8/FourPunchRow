using System;
using System.Collections;
using System.Collections.Generic;
using Logan.UI;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [SerializeField] private VRPlayer player;
    [SerializeField] private VRPlayer aiPlayer;
    
    [SerializeField] private StartCounterManager startCounterManager;
    [SerializeField] private TimeToNextPhase timeToNextPhase;

    [SerializeField] private GameObject fourInRowObject;

    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("A GameManager is already in the scene");
            return;
        }

        Singleton = this;
    }
    
    void Start()
    {
        startCounterManager.GetGameCanBeginEvent().AddListener(OnGameCanBegin);
        timeToNextPhase.EvtChangeModeToBoxing().AddListener(OnChangeModeToBoxing);
        timeToNextPhase.EvtChangeModeToFour().AddListener(OnChangeModeToFourInRows);
    }

    private void OnGameCanBegin()
    {
        timeToNextPhase.OnGameReallyStarts();
        Debug.Log("Game can begin!");
        OnChangeModeToFourInRows();
    }
    private void OnChangeModeToFourInRows()
    {
        Debug.Log("Change mode to four in rows!");
        fourInRowObject.SetActive(true);
        player.SetPlayerMode(VRPlayer.PlayerMode.Interacting);
        aiPlayer.SetPlayerMode(VRPlayer.PlayerMode.Interacting);
    }
    private void OnChangeModeToBoxing()
    {
        Debug.Log("Change mode to boxing!");
        fourInRowObject.SetActive(false);
        player.SetPlayerMode(VRPlayer.PlayerMode.Boxing);
        aiPlayer.SetPlayerMode(VRPlayer.PlayerMode.Boxing);
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
