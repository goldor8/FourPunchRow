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
        [SerializeField] private AudioClip[] sndBoxingMode;
        [SerializeField] private AudioClip[] sndRowsMode;
        [SerializeField] private AudioClip sndPunchOne;
        [SerializeField] private AudioClip sndPunchTwo;
        [SerializeField] private AudioClip sndPunchThree;
        [SerializeField] private AudioClip[] sndWinBoxing;
        [SerializeField] private AudioClip[] sndGameOver;

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

        public void PlayBoxingModeSound()
        {
            foreach (var sound in sndBoxingMode)
            {
                audioData.PlayOneShot(sound, 0.5F);                    
            }
        }
        public void PlayFourRowsModeSound()
        {
            foreach (var sound in sndRowsMode)
            {
                audioData.PlayOneShot(sound, 0.5F);                    
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
            foreach (var sound in sndWinBoxing)
            {
                audioData.PlayOneShot(sound, 0.5F);                    
            }
        }
        public void PlayGameOverSound()
        {
            foreach (var sound in sndGameOver)
            {
                audioData.PlayOneShot(sound, 0.5F);                    
            }
        }
    }
    
}