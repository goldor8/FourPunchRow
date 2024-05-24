using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractor
{
    public void RegisterOnInteract(UnityAction<InteractEvent, IInteractor> listener);
    public void UnregisterOnInteract(UnityAction<InteractEvent, IInteractor> listener);
}
