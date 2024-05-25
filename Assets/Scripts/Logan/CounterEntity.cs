using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class CounterEntity : MonoBehaviour
{
    AudioSource audioData;
    public AudioClip counterSound;
    public AudioClip finalCounterSound;
    
    private UnityEvent gameCanBeginEvent = new UnityEvent();
    private double timeSinceLastSecond = 0;
    private int counter = 0;
    

    public UnityEvent GetGameCanBeginEvent()
    {
        return gameCanBeginEvent;
    }

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        timeSinceLastSecond += Time.deltaTime;
        if (timeSinceLastSecond is 0 or >= 1)
        {
            timeSinceLastSecond = 0;
            OnTick();
            counter++;
        }
    }

    private void OnTick()
    {
        switch (counter) 
        {
            case 0 or 1 or 2:
                PlayCounterSound();
                break;
            case 3:
                PlayFinalCounterSound();
                gameCanBeginEvent.Invoke();
                break;
            default:
                DestroyMe();
                break;
        };
    }

    private void PlayCounterSound()
    {
        if (counterSound)
        {
            audioData.PlayOneShot(counterSound, 0.7F);
        }
    }
    private void PlayFinalCounterSound()
    {
        if (finalCounterSound)
        {
            audioData.PlayOneShot(finalCounterSound, 0.7F);
        }
    }
    private void DestroyMe()
    {
        Debug.Log("Finished !");
        Destroy(this);
    }
}
