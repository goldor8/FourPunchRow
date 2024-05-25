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
        [SerializeField] private AudioClip sndPunchOne;
        [SerializeField] private AudioClip sndPunchTwo;
        [SerializeField] private AudioClip sndPunchThree;
        [SerializeField] private AudioClip sndPunchFinal;
        [SerializeField] private AudioClip sndPeopleCheer;

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
                audioData.PlayOneShot(sndDing, 0.5F);
            }
        }
        public void PlayRowsSound()
        {
            if (sndRows)
            {
                audioData.PlayOneShot(sndRows, 0.7F);
            }
        }

        public void PlayRandomPunchSound()
        {
            if (sndPunchOne && sndPunchTwo  && sndPunchThree)
            {
                AudioClip soundToPlay = Random.Range(0, 12) switch {
                    < 2 => sndPunchThree,
                    > 2 and <= 6 => sndPunchTwo,
                    _ => sndPunchOne
                };
                audioData.PlayOneShot(soundToPlay, 0.7F);
            }
        }

        public void PlayBoxingWinSound()
        {
            if (sndPunchFinal && sndPeopleCheer)
            {
                audioData.PlayOneShot(sndPunchFinal, 0.7F);   
                audioData.PlayOneShot(sndPeopleCheer, 0.7F);   
            }
        }
    }
    
}