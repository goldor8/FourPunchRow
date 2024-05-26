using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVRPlayer : VRPlayer
{
    protected override void Start()
    {
        GameManager.Singleton.SetAIPlayer(this);
    }

    private void Update()
    {
        
    }
}
