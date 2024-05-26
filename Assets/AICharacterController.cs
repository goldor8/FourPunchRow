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
    [SerializeField] private Vector2 punchDelay;

    private float leftHandCooldown;
    private float rightHandCooldown;

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
            float currentAngle = aiBody.rotation.eulerAngles.y;
            float delta = Mathf.DeltaAngle(currentAngle, angleToTarget);
            Vector2 direction = Vector2.up * Mathf.Clamp(Vector3.Dot(-Vector3.forward, playerDifference), 0, 1);
            Move(direction, Mathf.Clamp(delta, -1, 1));
        }
        else
        {
            Debug.Log("try try punch");
            TryPunch();    
        }
    }

    private void TryPunch()
    {
        if (punchCooldown < Time.time)
        {
            punchCooldown = Time.time + random.NextFloat(punchDelay.x, punchDelay.y);
            Debug.Log("try punch");
            StartCoroutine(Punch());
        }
    }

    IEnumerator Punch()
    {
        Debug.Log("punch");
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
        }
        else if (leftHandCooldown < Time.time)
        {
            punchHand = leftHand;
        }
        else if (rightHandCooldown < Time.time)
        {
            rightSide = true;
            punchHand = rightHand;
        }
        else
        {
            yield return null;
        }

        float punchStep = 16;
        float punchTime = 0.6f;
        float progress = 0;
        Vector3 startPos = punchHand.position;
        Vector3 lastPos = punchHand.position;
        Vector3 targetPos = target.position;
        while (progress < 1)
        {
            progress += 1 / punchStep;
            punchHand.position = Vector3.Slerp(startPos, targetPos, progress);
            punchHand.rotation = Quaternion.LookRotation(punchHand.position - lastPos);
            lastPos = punchHand.position;
            yield return new WaitForSeconds(punchTime / punchStep);
        }

        progress = 0;
        while (progress < 1)
        {
            progress += 1 / punchStep;
            punchHand.position = Vector3.Slerp(targetPos, startPos, progress);
            punchHand.rotation = Quaternion.LookRotation(punchHand.position - lastPos);
            lastPos = punchHand.position;
            yield return new WaitForSeconds(punchTime / punchStep);
        }

        punchHand.position = rightSide ? rightHandDefaultPos.position : leftHandDefaultPos.position;
        punchHand.rotation = rightSide ? rightHandDefaultPos.rotation : leftHandDefaultPos.rotation;
    }
}