using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public float minPauseTime = 2f;
    public float maxPauseTime = 5f;
    public float minMovementDistance = 5f;
    public float maxMovementDistance = 15f;
    public float minMovementSpeed = 3f;
    public float maxMovementSpeed = 6f;

    private Vector3 randomTargetPosition;
    private float currentMovementSpeed;
    private bool isMoving = true;

    private void Start()
    {
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
        StartMovement();
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToTargetPosition();
        }
        else
        {
            PauseAtTargetPosition();
        }
    }

    private void StartMovement()
    {
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
        isMoving = true;
    }

    private void CaughtByPlayer()
    {
        isMoving = false;

        GenerateRandomTargetPosition();

        Invoke("RestartMovement", Random.Range(minPauseTime, maxPauseTime));
    }

    private void MoveToTargetPosition()
    {
        Vector3 direction = randomTargetPosition - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= 0.1f)
        {
            StartPause();
        }
        else
        {
            Vector3 movement = direction.normalized * currentMovementSpeed * Time.deltaTime;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, movement, out hit, movement.magnitude))
            {
               
                Vector3 avoidanceDirection = Vector3.Reflect(movement.normalized, hit.normal);
                movement = avoidanceDirection * currentMovementSpeed * Time.deltaTime;
            }

            transform.position += movement;
        }
    }

    private void PauseAtTargetPosition()
    {
     
    }

    private void StartPause()
    {
        isMoving = false;
        float pauseTime = Random.Range(minPauseTime, maxPauseTime);
        Invoke("RestartMovement", pauseTime);
    }

    private void RestartMovement()
    {
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
        isMoving = true;
    }

    private void GenerateRandomTargetPosition()
    {
        float randomX = Random.Range(-maxMovementDistance, maxMovementDistance);
        float randomZ = Random.Range(-maxMovementDistance, maxMovementDistance);
        randomTargetPosition = new Vector3(randomX, 0f, randomZ);
    }

    private void GenerateRandomMovementSpeed()
    {
        currentMovementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }
}
