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
        [SerializeField] private AnimatedOverlayCmp counterOverlay;

        private readonly UnityEvent gameCanBeginEvent = new();
        private readonly Countdown counter = new(3);
        
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
                case 3:
                    counterOverlay.ShowAnimated(true);
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    break;
                case 2 or 1:
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    break;
                default:
                    counterOverlay.Hide();
                    soundManager.PlayFinalCounterSound();
                    gameCanBeginEvent.Invoke();
                    this.enabled = false;
                    break;
            }
        }

        private void SetSpriteByCounter()
        {
            if (counter.Value > 0)
            {
                counterOverlay.SetSprite(sprites[3 - counter.Value]);
            }
        }
    }
}