using Logan.Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Logan.UI
{
    public class StartCounterManager : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private Sprite[] sprites = new Sprite[3];
        [SerializeField] private Image imageDisplay;
        [SerializeField] private PlayableDirector displayPlayableDirector;

        private readonly UnityEvent gameCanBeginEvent = new();
        private readonly Countdown counter = new(4);
        
        public UnityEvent GetGameCanBeginEvent()
        {
            return gameCanBeginEvent;
        }

        void Start()
        {
            counter.Start();
        }

        void Update()
        {
            if (counter.CanTick(Time.deltaTime))
            {
                OnTick();
            }
        }

        private void OnTick()
        {
            switch (counter.Value)
            {
                case 4:
                    displayPlayableDirector.Play();
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    break;
                case 3 or 2:
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    this.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
                    break;
                case 1:
                    SetSpriteByCounter();
                    soundManager.PlayFinalCounterSound();
                    soundManager.PlayDingDingDingSound();
                    this.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
                    break;
                default:
                    displayPlayableDirector.Stop();
                    imageDisplay.enabled = false;
                    gameCanBeginEvent.Invoke();
                    this.enabled = false;
                    break;
            }
        }

        private void SetSpriteByCounter()
        {
            if (counter.Value > 0)
            {
                imageDisplay.sprite = sprites[4 - counter.Value];
            }
        }
    }
}