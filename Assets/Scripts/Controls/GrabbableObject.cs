using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IInteractable
{
    public bool teleported = false;
    private Transform originalParent;
    private Rigidbody objectRigidbody;
    public bool isOxygen;
    private Vector3 lastPosition;
    private Vector3 currentVelocity;
    private RigidbodyData previousData;
    private int previousLayer;
    private HashSet<FixedJoint> grabbers = new HashSet<FixedJoint>();

    struct RigidbodyData
    {
        public bool isKinematic;
        public float drag;
        public float angularDrag;
    }
    
    void Start()
    {
        originalParent = transform.parent;
        objectRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        currentVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
        lastPosition = transform.position;
    }

    public void Grab(InteractEvent @event)
    {
        
        // previousLayer = gameObject.layer;
        // CopyDataFromRigidbody(objectRigidbody, previousData);
        // gameObject.layer = 7;
        // objectRigidbody.isKinematic = true;
        // // objectRigidbody.drag = 0;
        // // objectRigidbody.angularDrag = 0;
        // transform.SetParent(@event.Source.GameObject.transform);
        FixedJoint joint = @event.Source.GameObject.AddComponent<FixedJoint>();
        joint.connectedBody = objectRigidbody;
        grabbers.Add(joint);

        @event.Results.Add(HandAnimator.FreezeHandAnimation);
    }

    public void OnUniversalInteractRelease(InteractEvent @event, IInteractor interactor)
    {
        if (@event.Type == XRInteractor.VRRelease)
            Release(@event);
    }

    public void Release(InteractEvent @event)
    {
        foreach (FixedJoint fixedJoint in grabbers.Where(joint => joint.gameObject == @event.Source.GameObject && joint.connectedBody == objectRigidbody))
        {
            Destroy(fixedJoint);
            grabbers.Remove(fixedJoint);
        }

        @event.Results.Add(HandAnimator.UnfreezeHandAnimation);
        //if (grabbers.Count != 0) return;
        // transform.SetParent(originalParent);
        // gameObject.layer = previousLayer;
        // CopyDataToRigidbody(objectRigidbody, previousData);
        // objectRigidbody.velocity = currentVelocity;
    }

    public void OnInteract(InteractEvent @event, IInteractor interactor)
    {
        if (@event.Type == XRInteractor.VRGrab)
            Grab(@event);

        interactor.RegisterOnInteract(OnUniversalInteractRelease); // Register on the global event (in some cases, this object isn't in the range of detection of the interactor)
    }

    private void CopyDataFromRigidbody(Rigidbody rigidbody, RigidbodyData data)
    {
        data.isKinematic = rigidbody.isKinematic;
        data.drag = rigidbody.drag;
        data.angularDrag = rigidbody.angularDrag;
    }

    private void CopyDataToRigidbody(Rigidbody rigidbody, RigidbodyData data)
    {
        rigidbody.isKinematic = data.isKinematic;
        rigidbody.drag = data.drag;
        rigidbody.angularDrag = data.angularDrag;
    }
}
