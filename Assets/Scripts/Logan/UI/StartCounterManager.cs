using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Logan.UI
{
    public class CounterEntity : MonoBehaviour
    {
        public SoundManager soundManager;
        public Sprite[] sprites = new Sprite[3];
        public Image imageDisplay;
        public PlayableDirector displayPlayableDirector;

        private readonly UnityEvent gameCanBeginEvent = new UnityEvent();
        private double timeSinceLastSecond = 0;
        private int counter = 0;


        public UnityEvent GetGameCanBeginEvent()
        {
            return gameCanBeginEvent;
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
                case 0:
                    displayPlayableDirector.Play();
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    break;
                case 1 or 2:
                    SetSpriteByCounter();
                    soundManager.PlayCounterSound();
                    this.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
                    break;
                case 3:
                    SetSpriteByCounter();
                    soundManager.PlayFinalCounterSound();
                    this.transform.localScale -= new Vector3(0.25f, 0.25f, 0.25f);
                    break;
                default:
                    displayPlayableDirector.Stop();
                    imageDisplay.enabled = false;
                    this.enabled = false;
                    break;
            }
        }

        private void SetSpriteByCounter()
        {
            imageDisplay.sprite = sprites[counter];
        }
    }
}