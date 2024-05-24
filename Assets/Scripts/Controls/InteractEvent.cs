using System.Collections.Generic;
using UnityEngine;

public record InteractEvent
{
    public class InteractSource
    {
        public GameObject GameObject { get; }
        public InteractSource(GameObject gameObject)
        {
            this.GameObject = gameObject;
        }
    }
    public class InteractResult { }
    public class InteractionType { }
    
    public InteractionType Type { get; }
    public InteractSource Source { get; }
    
    public HashSet<InteractResult> Results { get; } = new HashSet<InteractResult>();


    public InteractEvent(InteractSource interactSource, InteractionType interactType)
    {
        this.Source = interactSource;
        this.Type = interactType;
    }
}
