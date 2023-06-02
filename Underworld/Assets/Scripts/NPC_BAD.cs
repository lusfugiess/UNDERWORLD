using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BAD : MonoBehaviour
{
    public string playerTag = "Player";
    public float followRadius = 50f;
    public float stopDuration = 10f;
    public float moveSpeed = 6f;
    public float randomMovementRadius = 100f;

    private Transform playerTransform;
    private Vector3 randomTargetPosition;
    private float currentMovementSpeed;
    private bool isFollowingPlayer = false;
    private bool isStopped = false;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object with tag '" + playerTag + "' not found!");
        }

        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= followRadius)
            {
                isFollowingPlayer = true;
                MoveTowardsPlayer();
            }
            else if (distance > followRadius && isFollowingPlayer)
            {
                isFollowingPlayer = false;
                GenerateRandomTargetPosition();
                GenerateRandomMovementSpeed();
            }
        }

        if (!isFollowingPlayer && !isStopped)
        {
            MoveRandomly();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > followRadius)
        {
            GenerateRandomMovementSpeed();
        }

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;
        transform.position += direction.normalized * currentMovementSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);
        }
    }

    private void MoveRandomly()
    {
        if (randomTargetPosition == null || Vector3.Distance(transform.position, randomTargetPosition) <= 0.1f)
        {
            GenerateRandomTargetPosition();
            StartCoroutine(StopForDuration(stopDuration));
        }

        Vector3 direction = randomTargetPosition - transform.position;
        direction.y = 0f;
        transform.position += direction.normalized * currentMovementSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360f);
        }
    }

    private void GenerateRandomTargetPosition()
    {
        float randomX = Random.Range(-randomMovementRadius, randomMovementRadius);
        float randomZ = Random.Range(-randomMovementRadius, randomMovementRadius);
        randomTargetPosition = new Vector3(randomX, 0f, randomZ);
    }

    private void GenerateRandomMovementSpeed()
    {
        currentMovementSpeed = moveSpeed;
    }

    private IEnumerator StopForDuration(float duration)
    {
        isStopped = true;
        yield return new WaitForSeconds(duration);
        isStopped = false;
    }
}
