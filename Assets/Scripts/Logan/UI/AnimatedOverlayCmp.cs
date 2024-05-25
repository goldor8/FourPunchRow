using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(PlayableDirector))]
public class AnimatedOverlayCmp : MonoBehaviour
{
    private Image sprite;
    private PlayableDirector playableDirector;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<Image>();
        playableDirector = GetComponent<PlayableDirector>();
        sprite.enabled = false;
    }

    public void ShowAnimated(bool withLoop = false)
    {
        sprite.enabled = true;
        playableDirector.extrapolationMode = (withLoop) ? DirectorWrapMode.Loop : DirectorWrapMode.Hold;
        playableDirector.Play();
    }
    public void SetSprite(Sprite sprite)
    {
        this.sprite.sprite = sprite;
    }
    public void Hide()
    {
        playableDirector.Stop();
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
