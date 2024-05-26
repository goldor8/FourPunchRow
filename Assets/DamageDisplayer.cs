using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplayer : MonoBehaviour
{
    [SerializeField] private string prefix;
    private TMP_Text textContainer;

    private void Awake()
    {
        textContainer = GetComponent<TMP_Text>();
    }

    public void ActualiseDamage(int damages)
    {
        textContainer.text = prefix + damages;
    }
}
