using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BAD : MonoBehaviour
{
    public Transform playerTransform;
    public float followRadius = 2f;
    public float stopRadius = 1f;
    public float minMovementSpeed = 3f;
    public float maxMovementSpeed = 6f;
    public float randomMovementRadius = 5f;

    private bool isFollowingPlayer = false;
    private Vector3 randomTargetPosition;
    private float currentMovementSpeed;

    private void Start()
    {
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
    }

    private void Update()
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

        if (!isFollowingPlayer)
        {
            MoveRandomly();
        }
    }

 private void MoveTowardsPlayer()
{
    float distance = Vector3.Distance(transform.position, playerTransform.position);

    if (distance > stopRadius)
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f; 
        transform.position += direction.normalized * currentMovementSpeed * Time.deltaTime;
    }
}


    private void MoveRandomly()
    {
        Vector3 direction = randomTargetPosition - transform.position;
        direction.y = 0f; 

        if (direction.magnitude <= 0.1f)
        {
            GenerateRandomTargetPosition();
        }
        else
        {
            transform.position += direction.normalized * currentMovementSpeed * Time.deltaTime;
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
        currentMovementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }
}
