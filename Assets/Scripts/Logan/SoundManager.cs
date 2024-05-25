using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logan
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager: MonoBehaviour
    {
        AudioSource audioData;
        [SerializeField] private AudioClip sndCounter;
        [SerializeField] private AudioClip sndCounterFinal;
        [SerializeField] private AudioClip sndDing;
        [SerializeField] private AudioClip sndRows;

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
        public void PlayRowsSound()
        {
            if (sndRows)
            {
                audioData.PlayOneShot(sndRows, 0.7F);
            }
        }
    }
    
}