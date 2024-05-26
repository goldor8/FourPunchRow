using System.Collections;
using System.Collections.Generic;
using Logan;
using Logan.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeToNextPhase : MonoBehaviour
{
    private const int TIME_TO_NEXT_PHASE = 10;

    [SerializeField] private TextMeshProUGUI timeToNextPhaseText;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AnimatedOverlayCmp phaseBoxingOverlay;
    [SerializeField] private AnimatedOverlayCmp phaseFourInRowsOverlay;
    
    private readonly Countdown countdown = new(TIME_TO_NEXT_PHASE);
    private readonly UnityEvent ringGotoBoxingEvent = new();
    private readonly UnityEvent ringGotoFourInRowsEvent = new();
    private bool isBoxing = true;

    public void OnGameReallyStarts()
    {
        countdown.Start();
        soundManager.PlayBoxingModeSound();
        phaseBoxingOverlay.ShowAnimated();
    }
    public UnityEvent EvtChangeModeToBoxing()
    {
        return ringGotoBoxingEvent;
    }
    public UnityEvent EvtChangeModeToFour()
    {
        return ringGotoFourInRowsEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown.CanTick(Time.deltaTime))
        {
            OnTick();
        }  
    }

    private void OnTick()
    {
        int secondsRemaining = countdown.Value;
        timeToNextPhaseText.SetText("" + secondsRemaining);
        HideOverlayIfRequired(secondsRemaining);
        if (secondsRemaining == 0)
        {
            switchGameMode();
        }
    }

    private void switchGameMode()
    {
        if (isBoxing)
        {
            soundManager.PlayFourRowsModeSound();
            ringGotoBoxingEvent.Invoke();
            phaseFourInRowsOverlay.ShowAnimated();
        }
        else
        {
            soundManager.PlayBoxingModeSound();
            ringGotoFourInRowsEvent.Invoke();
            phaseBoxingOverlay.ShowAnimated();
        }
        isBoxing = !isBoxing;
        countdown.Start();
    }
    private void HideOverlayIfRequired(int secondsRemaining)
    {
        if (secondsRemaining == TIME_TO_NEXT_PHASE - 1)
        {
            phaseBoxingOverlay.Hide();
            phaseFourInRowsOverlay.Hide();
        }
    }

}
