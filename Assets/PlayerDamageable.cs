using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : MonoBehaviour
{
    [SerializeField] private int damageOnHit;
    [SerializeField] private VRPlayer vrPlayer;
    
    public void MarkHit()
    {
        vrPlayer.Damage(damageOnHit);
    }
}
