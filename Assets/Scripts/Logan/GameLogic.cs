using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    // pub
    private UnityEvent ringGotoBoxingEvent;
    private UnityEvent ringGotoFourInRowsEvent;
    
    // sub
    public CounterEntity counterEntity;
    private UnityEvent<int> playerOneDamageEvent;
    private UnityEvent playerOneKilled;
    
    void Start()
    {
        counterEntity.GetGameCanBeginEvent().AddListener(onGameCanBegin);
    }

    private void onGameCanBegin()
    {
        // log
        Debug.Log("GameLogic: onGameCanBegin");
    }

    void Update()
    {
        
    }
}
