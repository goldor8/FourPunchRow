using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(VRPlayer))]
public class XRInteractor : MonoBehaviour, IInteractor
{
    public static InteractEvent.InteractionType VRGrab = new InteractEvent.InteractionType();
    public static InteractEvent.InteractionType VRRelease = new InteractEvent.InteractionType();
    
    [SerializeField] private VRPlayer player;
    public CollisionDetection leftHand;
    public CollisionDetection rightHand;
    
    public InputActionProperty leftHandGripAction;
    public InputActionProperty rightHandGripAction;

    public bool leftHandGrabbing;
    public bool rightHandGrabbing;
    
    public UnityEvent<InteractEvent, IInteractor> OnInteract;
    public UnityEvent<InteractEvent, IInteractor> OnRightHandInteract;
    public UnityEvent<InteractEvent, IInteractor> OnLeftHandInteract;
    
    private List<(GameObject grabbed, bool rightHand)> grabbedObjects = new();

    private void FixedUpdate()
    {
        //CheckIfGrabbedObjectAreStillGrabbed();
        
        bool isLeftHandGrabbing = leftHandGripAction.action.ReadValue<float>() > 0.5f;
        bool isRightHandGrabbing = rightHandGripAction.action.ReadValue<float>() > 0.5f;

        SetRightHandState(isRightHandGrabbing);
        SetLeftHandState(isLeftHandGrabbing);
    }
    
    private void SetRightHandState(bool grabbing)
    {
        if (rightHandGrabbing == grabbing) return;

        HandInteract(rightHand, grabbing, true);
        
        rightHandGrabbing = grabbing;
    }

    private void SetLeftHandState(bool grabbing)
    {
        if (leftHandGrabbing == grabbing) return;
        
        HandInteract(leftHand, grabbing, false);
        
        leftHandGrabbing = grabbing;
    }
    
    private void HandInteract(CollisionDetection hand, bool grabbing, bool rightHand)
    {
        Collider[] colliders = hand.SphereCastCollisions();
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].TryGetComponent(out IInteractable interactable)) continue;

            InteractEvent @event = new InteractEvent(new InteractEvent.InteractSource(hand.transform.parent.gameObject), grabbing ? VRGrab : VRRelease);
            interactable.OnInteract(@event, this);
            
            if(grabbing)
                grabbedObjects.Add((colliders[i].gameObject, rightHand));
            else
                grabbedObjects.RemoveAll(obj => obj.grabbed == colliders[i].gameObject);
            
            if(rightHand)
                OnRightHandInteract.Invoke(@event, this);
            else
                OnLeftHandInteract.Invoke(@event, this);

            OnInteract.Invoke(@event, this);
            
            return;
        }
    }

    private void CheckIfGrabbedObjectAreStillGrabbed()
    {
        Collider[] rightHandColliders = rightHand.SphereCastCollisions();
        Collider[] leftHandColliders = leftHand.SphereCastCollisions();
        
        for (int i = 0; i < grabbedObjects.Count; i++)
        {
            Debug.Log($"Checking {grabbedObjects[i].grabbed.name} object is still grabbed");
            if (grabbedObjects[i].rightHand)
            {
                Debug.Log("Right hand colliders :");
                foreach (var collider in rightHandColliders)
                {
                    Debug.Log(collider.gameObject.name);
                }
                
                if (rightHandColliders.Any(collider => collider.gameObject == grabbedObjects[i].grabbed)) continue;
            }
            else
            {
                Debug.Log("Left hand colliders :");
                foreach (var collider in leftHandColliders)
                {
                    Debug.Log(collider.gameObject.name);
                }
                
                if (leftHandColliders.Any(collider => collider.gameObject == grabbedObjects[i].grabbed)) continue;
            }
            
            Debug.Log("Release object");
            if (grabbedObjects[i].grabbed.TryGetComponent(out IInteractable interactable))
            {
                var interactEvent = new InteractEvent(new InteractEvent.InteractSource(grabbedObjects[i].rightHand ? rightHand.gameObject : leftHand.gameObject), VRRelease);
                interactable.OnInteract(interactEvent, this);
                
                if(grabbedObjects[i].rightHand)
                    OnRightHandInteract.Invoke(interactEvent, this);
                else
                    OnLeftHandInteract.Invoke(interactEvent, this);
                OnInteract.Invoke(interactEvent, this);
            }
            
            grabbedObjects.RemoveAt(i);
            i--;
        }
    }

    public void RegisterOnInteract(UnityAction<InteractEvent, IInteractor> listener)
    {
        OnInteract.AddListener(listener);
    }
    
    public void UnregisterOnInteract(UnityAction<InteractEvent, IInteractor> listener)
    {
        OnInteract.RemoveListener(listener);
    }
}
