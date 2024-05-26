using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class AICharacterController : CharacterController
{
    [SerializeField] private float handsRange;

    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform leftHandDefaultPos;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform rightHandDefaultPos;
    
    [Header("Combat config")]
    [SerializeField] private Vector2 punchDelay;
    [SerializeField] private float punchTime;
    [SerializeField] private int punchStep;

    private float leftHandCooldown;
    private bool cancelLeftHand;
    private Coroutine leftHandCoroutine;
    private float rightHandCooldown;
    private bool cancelRightHand;
    private Coroutine rightHandCoroutine;

    private float punchCooldown;
    
    Random random = new Random(5);
    
    private void FixedUpdate()
    {
        VRPlayer player = GameManager.Singleton.GetPlayer();
        VRPlayer aiPlayer = GameManager.Singleton.GetAIPlayer();
        Transform playerBody = player.GetBodyTransform();
        Transform aiBody = aiPlayer.GetBodyTransform();

        Vector3 playerDifference = playerBody.position - aiBody.position;

        if (playerDifference.magnitude > handsRange)
        {
            float angleToTarget = Mathf.Atan2(playerDifference.x, playerDifference.z) * Mathf.Rad2Deg;
            Debug.DrawLine(aiBody.transform.position, aiBody.transform.position + playerDifference, Color.red);
            float currentAngle = aiBody.rotation.eulerAngles.y;
            float delta = Mathf.DeltaAngle(currentAngle, angleToTarget);
            Vector2 direction = Vector2.up * Mathf.Clamp(Vector3.Dot(Vector3.forward, aiBody.transform.InverseTransformDirection(playerDifference)), 0, 1);
            Move(direction, Mathf.Clamp(delta, -1, 1));
            Debug.DrawLine(aiBody.transform.position, aiBody.transform.position + new Vector3(direction.x, 0, direction.y));
        }
        else
        {
            TryPunch();    
        }
    }

    private void TryPunch()
    {
        if (punchCooldown < Time.time)
        {
            punchCooldown = Time.time + random.NextFloat(punchDelay.x, punchDelay.y);
            VRPlayer player = GameManager.Singleton.GetPlayer();
            Transform playerHead = player.GetHeadTransform();
            Transform playerBody = player.GetBodyTransform();

            Transform target = random.NextDouble() < 0.5f ? playerHead : playerBody;
            Transform punchHand = null;

            bool rightSide = false;
        
            if (leftHandCooldown < Time.time && rightHandCooldown < Time.time)
            {
                rightSide = random.NextDouble() < 0.5f;
                punchHand = rightSide ? rightHand : leftHand;
                if (rightSide)
                {
                    rightHandCooldown = Time.time + 2 * punchTime;
                }
                else
                {
                    leftHandCooldown = Time.time + 2 * punchTime;
                }
            }
            else if (leftHandCooldown < Time.time)
            {
                punchHand = leftHand;
                leftHandCooldown = Time.time + 2 * punchTime;
            }
            else if (rightHandCooldown < Time.time)
            {
                rightSide = true;
                punchHand = rightHand;
                rightHandCooldown = Time.time + 2 * punchTime;
            }
            else
            {
                return;
            }

            if (rightSide)
            {
                rightHandCoroutine = StartCoroutine(Punch(punchHand, target, rightHandDefaultPos));
            }
            else
            {
                leftHandCoroutine = StartCoroutine(Punch(punchHand, target, leftHandDefaultPos));
            }
            
        }
    }

    IEnumerator Punch(Transform punchHand, Transform target, Transform defaultHandPosition)
    {
        float progress = 0;
        Vector3 startPos = punchHand.position;
        Vector3 lastPos = punchHand.position;
        Vector3 targetPos = target.position;
        while (progress < 1)
        {
            if (cancelRightHand && punchHand == rightHand)
            {
                Debug.Log("cancelling right");
                cancelRightHand = false;
                break;   
            }

            if (cancelLeftHand && punchHand == leftHand)
            {
                Debug.Log("cancelling left");
                cancelLeftHand = false;
                break;
            }
            progress += 1f / punchStep;
            punchHand.position = Vector3.Slerp(startPos, targetPos, progress);
            punchHand.rotation = Quaternion.LookRotation(punchHand.position - lastPos);
            lastPos = punchHand.position;
            yield return new WaitForSeconds(punchTime / punchStep);
        }
        
        while (progress > 1)
        {
            progress -= 1f / punchStep;
            punchHand.position = Vector3.Slerp(targetPos, startPos, progress);
            punchHand.rotation = Quaternion.LookRotation(punchHand.position - lastPos);
            lastPos = punchHand.position;
            yield return new WaitForSeconds(punchTime / punchStep);
        }

        punchHand.position = defaultHandPosition.position;
        punchHand.rotation = defaultHandPosition.rotation;
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying)
            return;
        VRPlayer player = GameManager.Singleton.GetPlayer();
        Transform playerHead = player.GetHeadTransform();
        Transform playerBody = player.GetBodyTransform();

        Transform target = playerHead;
        Transform punchHand = leftHand;

        DrawSlerp(punchHand.position, target.position);
        
        punchHand = rightHand;
        DrawSlerp(punchHand.position, target.position);

        target = playerBody;
        punchHand = leftHand;

        DrawSlerp(punchHand.position, target.position);
        
        punchHand = rightHand;
        DrawSlerp(punchHand.position, target.position);


    }

    private void DrawSlerp(Vector3 start, Vector3 target)
    {
        float progress = 0;
        Vector3 startPos = start;
        Vector3 lastPos = start;
        Vector3 targetPos = target;
        while (progress < 1)
        {
            progress += 1f / punchStep;
            Vector3 interpol = Vector3.Slerp(startPos, targetPos, progress);
            Gizmos.DrawLine(lastPos, interpol);
            lastPos = interpol;
        }
    }

    public void CancelPunch(bool rightHand)
    {
        if (rightHand)
        {
            cancelRightHand = true;
            Debug.Log("asking cancel right");
        }
        else
        {
            cancelLeftHand = true;
            Debug.Log("asking cancel left");
        }
    }
}