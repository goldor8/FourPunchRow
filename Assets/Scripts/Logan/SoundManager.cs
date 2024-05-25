using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logan
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager: MonoBehaviour
    {
        AudioSource audioData;
        public AudioClip sndCounter;
        public AudioClip sndCounterFinal;
        public AudioClip sndDing;

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

        public void PlayDingDingDingSound()
        {
            if (sndDing)
            {
                audioData.PlayOneShot(sndDing, 0.7F);
            }
        }
    }
    
}