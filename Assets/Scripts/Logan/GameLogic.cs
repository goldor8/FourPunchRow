using Logan.UI;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    
    // sub
    [SerializeField] private StartCounterManager startCounterManager;
    [SerializeField] private TimeToNextPhase timeToNextPhase;
    
    private UnityEvent<int> playerOneDamageEvent;
    private UnityEvent playerOneKilled;
    
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
    }
    private void OnChangeModeToFourInRows()
    {
        Debug.Log("Change mode to four in rows!");
    }
    private void OnChangeModeToBoxing()
    {
        Debug.Log("Change mode to boxing!");
    }

    void Update()
    {
        
    }
}
