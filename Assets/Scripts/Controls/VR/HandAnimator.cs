using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class HandAnimator : MonoBehaviour
{
    public static InteractEvent.InteractResult FreezeHandAnimation = new InteractEvent.InteractResult();
    public static InteractEvent.InteractResult UnfreezeHandAnimation = new InteractEvent.InteractResult();
    
    public enum Hand
    {
        Left,
        Right
    }
    
    public enum CollisionDetection
    {
        Sphere,
        Raycast
    }
    
    [SerializeField] private Hand hand;
    public bool thumbDown;
    public bool indexDown;
    public float grip;
    public float pinch;

    public LayerMask handIgnoreLayer;
    public CollisionDetection collisionDetection;
    public Animator handAnimator;
    [SerializeField] private float hapticTransitionTime = 0.05f;
    private int thumbUpLayer;
    private int indexUpLayer;
    
    public InputActionProperty thumbHapticAction;
    public InputActionProperty indexHapticAction;
    
    public InputActionProperty gripAction;
    public InputActionProperty pinchAction;

    public bool proceduralAnimation;
    [SerializeField] bool freezeHandPosition;
    
    [SerializeField] private float antiPhalanxRotation = 30;
    [SerializeField] private float maxPhalanxRotation = 81;
    [SerializeField] private float phalanxStepAngle = 1;
    [SerializeField] private float phalanxRaycastDistance = 0.008f;
    
    [Header("Hand Structure")]
    [SerializeField] private HandStructure handStructure;
    [SerializeField] private HandOffsets handOffsets;
    private void Awake()
    {
        thumbUpLayer = handAnimator.GetLayerIndex("ThumbUp");
        indexUpLayer = handAnimator.GetLayerIndex("IndexUp");

        thumbHapticAction.action.performed += context => SmoothLayerTransition(thumbUpLayer, 0f, hapticTransitionTime);
        indexHapticAction.action.started += context => SmoothLayerTransition(indexUpLayer, 0f, hapticTransitionTime);
        
        thumbHapticAction.action.canceled += context => SmoothLayerTransition(thumbUpLayer, 1f, hapticTransitionTime);
        indexHapticAction.action.canceled += context => SmoothLayerTransition(indexUpLayer, 1f, hapticTransitionTime);
        
        for(int i = 0; i < handStructure.fingers.Length; i++)
        {
            handStructure.fingers[i].positions = new Vector3[handStructure.fingers[i].PhalanxBones.Length];
            handStructure.fingers[i].rotations = new Quaternion[handStructure.fingers[i].PhalanxBones.Length];
            
            for (int j = 0; j < handStructure.fingers[i].positions.Length; j++)
            {
                handStructure.fingers[i].positions[j] = handStructure.fingers[i].PhalanxBones[j].localPosition;
                handStructure.fingers[i].rotations[j] = handStructure.fingers[i].PhalanxBones[j].localRotation;
            }
        }
        
        handOffsets.fingers = new FingerOffsets[handStructure.fingers.Length];
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            handOffsets.fingers[i].angles = new float[handStructure.fingers[i].PhalanxBones.Length];
            for (int j = 0; j < handStructure.fingers[i].PhalanxBones.Length; j++)
            {
                handOffsets.fingers[i].angles[j] = antiPhalanxRotation;
            }
        }
    }

    private void Update()
    {
        thumbDown = thumbHapticAction.action.IsPressed();
        indexDown = indexHapticAction.action.IsPressed();

        grip = gripAction.action.ReadValue<float>();
        pinch = pinchAction.action.ReadValue<float>();
        
        handAnimator.SetFloat("Grip", grip);
        handAnimator.SetFloat("Pinch", pinch);

        if(!proceduralAnimation) return;
        if (freezeHandPosition) return;
        Release();
        
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            for (int j = 0; j < handStructure.fingers[i].PhalanxBones.Length; j++)
            {
                GetPhalanxRotationToMakeContact(i, j);
            }
        }
        
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            bool specialPosition = false;
            for (int j = handStructure.fingers[i].PhalanxBones.Length-1; j >= 0; j--)
            {
                Transform phalanx = handStructure.fingers[i].PhalanxBones[j];
                if(handOffsets.fingers[i].angles[j] > grip * -maxPhalanxRotation)
                    specialPosition = true;
                float rotation = specialPosition ? handOffsets.fingers[i].angles[j] : grip * -maxPhalanxRotation;
                phalanx.localRotation = Quaternion.Euler(handStructure.fingers[i].rotations[j].eulerAngles + Vector3.forward * rotation);
            }
        }
    }
    
    public void FreezeHandPosition(bool freeze)
    {
        freezeHandPosition = freeze;
    }

    public void OnInteractEvent(InteractEvent @event)
    {
        bool interactionFromThisHand = @event.Source.GameObject == gameObject;
        
        if(!interactionFromThisHand) return;
        
        if(@event.Results.Contains(FreezeHandAnimation))
            FreezeHandPosition(true);
        
        if(@event.Results.Contains(UnfreezeHandAnimation))
            FreezeHandPosition(false);
    }
    
    private void SmoothLayerTransition(int layerIndex, float targetWeight, float duration)
    {
        if(!gameObject.activeSelf) return;
        IEnumerator transition()
        {
            float startWeight = handAnimator.GetLayerWeight(layerIndex);
            float startTime = Time.time;
            float endTime = startTime + duration;
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / duration;
                handAnimator.SetLayerWeight(layerIndex, Mathf.Lerp(startWeight, targetWeight, t));
                yield return new WaitForEndOfFrame();
            }
            handAnimator.SetLayerWeight(layerIndex, targetWeight);
        }
        
        StartCoroutine(transition());
    }

    public (Vector3 position, Vector3 direction) GetNewAbsolutePositionFromLocalPhalanx(int fingerIndex,
        int phalanxIndex, Vector3 localPosition, Vector3 localDirection)
    {
        for (int i = phalanxIndex; i >= 1; i--)
        {
            localPosition = Quaternion.Euler(0, 0, handOffsets.fingers[fingerIndex].angles[i]) * handStructure.fingers[fingerIndex].PhalanxBones[i].localRotation * localPosition;
            localDirection = Quaternion.Euler(0, 0, handOffsets.fingers[fingerIndex].angles[i]) * handStructure.fingers[fingerIndex].PhalanxBones[i].localRotation * localDirection;
            localPosition += handStructure.fingers[fingerIndex].PhalanxBones[i].localPosition;
        }
        
        localPosition = Quaternion.Euler(0, 0, handOffsets.fingers[fingerIndex].angles[0]) * localPosition;
        localDirection = Quaternion.Euler(0, 0, handOffsets.fingers[fingerIndex].angles[0]) * localDirection;
        Matrix4x4 LTWM = handStructure.fingers[fingerIndex].PhalanxBones[0].localToWorldMatrix;
        return (LTWM * ConvertToVector4Point(localPosition), LTWM * ConvertToVector4Direction(localDirection));
    }
    
    public bool IsPhalanxColliding(int fingerIndex, int phalanxIndex)
    {
        Transform phalanxRoot = handStructure.fingers[fingerIndex].PhalanxBones[phalanxIndex];
        Transform measurePoint = handStructure.fingers[fingerIndex].PhalanxMeasurePoints[phalanxIndex];
        Vector3 measurePointPosition = phalanxRoot.worldToLocalMatrix * ConvertToVector4Point(measurePoint.position);
        Vector3 measurePointForward = phalanxRoot.worldToLocalMatrix * ConvertToVector4Direction(measurePoint.forward);
        
        (Vector3 worldPosition, Vector3 worldDirection) = GetNewAbsolutePositionFromLocalPhalanx(fingerIndex, phalanxIndex, measurePointPosition, measurePointForward);
        int layerMask = Physics.AllLayers & ~handIgnoreLayer;
        bool colliding = false;
        switch (collisionDetection)
        {
            case CollisionDetection.Sphere:
                colliding = Physics.CheckSphere(worldPosition, phalanxRaycastDistance, layerMask);
                break;
            case CollisionDetection.Raycast:
                colliding = Physics.Raycast(worldPosition, worldDirection, phalanxRaycastDistance, layerMask);
                break;
        }
        return colliding;
    }

    void GetPhalanxRotationToMakeContact(int fingerIndex, int phalanxIndex)
    {
        float rotationStepAngle = -phalanxStepAngle;
        bool isColliding = false;
        for (int i = phalanxIndex; i < handStructure.fingers[fingerIndex].PhalanxBones.Length; i++)
        {
            if (IsPhalanxColliding(fingerIndex, i))
            {
                isColliding = true;
                break;
            }
        }
        while (!isColliding)
        {
            if(MathF.Abs(handOffsets.fingers[fingerIndex].angles[phalanxIndex]) > maxPhalanxRotation) break;
            
            handOffsets.fingers[fingerIndex].angles[phalanxIndex] += rotationStepAngle;
            //yield return new WaitForSeconds(0.1f);
            
            //check each bone
            for (int i = phalanxIndex; i < handStructure.fingers[fingerIndex].PhalanxBones.Length; i++)
            {
                if (IsPhalanxColliding(fingerIndex, i))
                {
                    isColliding = true;
                    break;
                }
            }
        }
    }

    
    IEnumerator MakeContactWithFinger(int fingerIndex)
    {
        // FingerStructure finger = handStructure.fingers[fingerIndex];
        // for (int i = 0; i < finger.PhalanxBones.Length; i++)
        // {
        //     CoroutineWithData coroutineWithData = new CoroutineWithData(this, GetPhalanxRotationToMakeContact(fingerIndex, i));
        //     yield return coroutineWithData.coroutine;
        //     float rotationAngle = (float) coroutineWithData.result;
        //     Debug.Log(rotationAngle);
        //     finger.PhalanxBones[i].localRotation = Quaternion.Euler(finger.rotations[i].eulerAngles + Vector3.forward * rotationAngle);
        //     handOffsets.fingers[fingerIndex].angles[i] = 0;
        // }
        yield return null;
    }

    [ContextMenu("test")]
    public void Test()
    {
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            for (int j = 0; j < handStructure.fingers[i].PhalanxBones.Length; j++)
            {
                GetPhalanxRotationToMakeContact(i, j);
            }
        }
            
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            for (int j = 0; j < handStructure.fingers[i].PhalanxBones.Length; j++)
            {
                Transform phalanx = handStructure.fingers[i].PhalanxBones[j];
                phalanx.localRotation = Quaternion.Euler(handStructure.fingers[i].rotations[j].eulerAngles + Vector3.forward * handOffsets.fingers[i].angles[j]);
            }
        }
    }
    
    [ContextMenu("release")]
    public void Release()
    {
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            for (int j = 0; j < handStructure.fingers[i].PhalanxBones.Length; j++)
            {
                handStructure.fingers[i].PhalanxBones[j].localPosition = handStructure.fingers[i].positions[j];
                handStructure.fingers[i].PhalanxBones[j].localRotation = handStructure.fingers[i].rotations[j];
            }
        }
        
        for (int i = 0; i < handOffsets.fingers.Length; i++)
        {
            for (int j = 0; j < handOffsets.fingers[i].angles.Length; j++)
            {
                handOffsets.fingers[i].angles[j] = antiPhalanxRotation;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(handStructure.fingers == null) return;
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            FingerStructure finger = handStructure.fingers[i];
            for (int j = 0; j < finger.PhalanxBones.Length; j++)
            {
                Transform phalanx = finger.PhalanxBones[j];
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(phalanx.position, 0.005f);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(finger.PhalanxMeasurePoints[j].position,finger.PhalanxMeasurePoints[j].position + finger.PhalanxMeasurePoints[j].forward * 0.002f);
            }
        }
        
        if(handOffsets.fingers.Length != handStructure.fingers.Length) return;
        for (int i = 0; i < handStructure.fingers.Length; i++)
        {
            FingerStructure finger = handStructure.fingers[i];
            if(handOffsets.fingers[i].angles.Length != finger.PhalanxMeasurePoints.Length) continue;
            for (int j = 0; j < finger.PhalanxMeasurePoints.Length; j++)
            {
                Gizmos.color = Color.blue;
                Vector3 localPosition = finger.PhalanxBones[j].worldToLocalMatrix * ConvertToVector4Point(finger.PhalanxMeasurePoints[j].position);
                Vector3 localDirection = finger.PhalanxBones[j].worldToLocalMatrix * ConvertToVector4Direction(finger.PhalanxMeasurePoints[j].forward);
                (Vector3 position, Vector3 direction) = GetNewAbsolutePositionFromLocalPhalanx(i, j, localPosition, localDirection);
                Gizmos.DrawLine(position,position + direction * 0.002f);
            }
        }
    }
    
    private Vector4 ConvertToVector4Point(Vector3 vector3)
    {
        return new Vector4(vector3.x, vector3.y, vector3.z, 1);
    }
    
    private Vector4 ConvertToVector4Direction(Vector3 vector3)
    {
        return new Vector4(vector3.x, vector3.y, vector3.z, 0);
    }

    [Serializable]
    struct HandStructure
    {
        [FormerlySerializedAs("Fingers")] public FingerStructure[] fingers;
    }
    
    [Serializable]
    struct FingerStructure
    {
        public Transform[] PhalanxBones;
        public Transform[] PhalanxMeasurePoints;
        [NonSerialized] public Vector3[] positions;
        [NonSerialized] public Quaternion[] rotations;
    }
    
    [Serializable]
    struct HandOffsets
    {
        public FingerOffsets[] fingers;
    }
    
    [Serializable]
    struct FingerOffsets
    {
        public float[] angles;
    }
}
