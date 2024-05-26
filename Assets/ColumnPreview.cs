using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnPreview : MonoBehaviour
{
    public static List<ColumnPreview> ColumnPreviews = new List<ColumnPreview>();
    
    [SerializeField] private int column;
    [SerializeField] private Puissance4 puissance4;
    private bool preview;
    [SerializeField] private GameObject previewObject;
    private GameObject instance;

    private void Awake()
    {
        ColumnPreviews.Add(this);
    }

    public void SetPreview(bool active)
    {
        if(active)
        {
            instance = Instantiate(previewObject);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
        }
        else
        {
            if(instance != null)
                Destroy(instance);
        }
    }

    public void Release()
    {
        puissance4.Play(column);
    }
}
