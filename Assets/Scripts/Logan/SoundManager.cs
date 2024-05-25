using UnityEngine;
using UnityEngine.Serialization;

namespace Logan.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager: MonoBehaviour
    {
        AudioSource audioData;
        public AudioClip sndCounter;
        public AudioClip sndCounterFinal;

        public void Start()
        {
            audioData = GetComponent<AudioSource>();
        }
        
        public void PlayCounterSound()
        {
            if (sndCounter)
            {
                audioData.PlayOneShot(sndCounter, 0.7F);
            }
        }
        public void PlayFinalCounterSound()
        {
            if (sndCounterFinal)
            {
                audioData.PlayOneShot(sndCounterFinal, 0.7F);
            }
        }
    }
    
}