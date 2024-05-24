using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public LayerMask grabbableLayers;
    public GameObject collidedObject;
    public List<GameObject> colliding = new List<GameObject>();
    [SerializeField] private float range;

    void OnTriggerEnter(Collider other)
    {
        if ((grabbableLayers.value & 1 << other.gameObject.layer) != 0)
        {
            collidedObject = other.gameObject;
        }
        
        colliding.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (collidedObject == other.gameObject)
        {
            collidedObject = null;
        }
        
        colliding.Remove(other.gameObject);
    }

    public Collider[] SphereCastCollisions()
    {
        return Physics.OverlapSphere(transform.position, range);
    }
    
    public RaycastHit? HandPalmRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            return hit;
        }
        
        return null;
    }
}
