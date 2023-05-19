using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rabbit : MonoBehaviour
{
    public float minPauseTime = 2f;
    public float maxPauseTime = 5f;
    public float minMovementDistance = 5f;
    public float maxMovementDistance = 15f;
    public float minMovementSpeed = 3f;
    public float maxMovementSpeed = 6f;
    public float catchRadius = 3f;
    public float catchTime = 2f;
    public TextMeshProUGUI rabbitsCaughtText;

    private Vector3 randomTargetPosition;
    private float currentMovementSpeed;
    private bool isMoving = true;
    private int rabbitsCaught = 0;
    private bool isBeingCaught = false;

    private void Start()
    {
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
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
            transform.position += direction.normalized * currentMovementSpeed * Time.deltaTime;
        }
    }

    private void PauseAtTargetPosition()
    {
        if (isBeingCaught)
        {
            catchTime -= Time.deltaTime;
            if (catchTime <= 0f)
            {
                Caught();
            }
        }
    }

    private void StartPause()
    {
        isMoving = false;
        float pauseTime = Random.Range(minPauseTime, maxPauseTime);
        Invoke("StartMovement", pauseTime);
    }

    private void StartMovement()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBeingCaught)
        {
            isBeingCaught = true;
            catchTime = 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBeingCaught = false;
        }
    }

    private void Caught()
    {
        isBeingCaught = false;
        rabbitsCaught++;
        rabbitsCaughtText.text = "Rabbits Caught: " + rabbitsCaught.ToString();
        DespawnRabbit();
    }

    private void DespawnRabbit()
    {
      
        float randomX = Random.Range(-maxMovementDistance, maxMovementDistance);
        float randomZ = Random.Range(-maxMovementDistance, maxMovementDistance);
        transform.position = new Vector3(randomX, 0f, randomZ);

       
        GenerateRandomTargetPosition();
        GenerateRandomMovementSpeed();
        isMoving = true;
    }
}
