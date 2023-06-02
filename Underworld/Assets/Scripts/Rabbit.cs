using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public float minPauseTime = 2f;
    public float maxPauseTime = 5f;
    public float minMovementSpeed = 3f;
    public float maxMovementSpeed = 6f;
    public float detectionRadius = 5f;
    public float speedMultiplier = 2f;

    private float currentMovementSpeed;
    private bool isMoving = true;
    private Transform player;

    private void Start()
    {
        GenerateRandomMovementSpeed();
        StartMovement();

        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveRandomly();
        }
    }

    private void StartMovement()
    {
        GenerateRandomMovementSpeed();
        isMoving = true;
    }

    private void CaughtByPlayer()
    {
        isMoving = false;

        Invoke("RestartMovement", Random.Range(minPauseTime, maxPauseTime));
    }

    private void MoveRandomly()
    {
        Vector3 movement = transform.forward * currentMovementSpeed * Time.deltaTime;
        movement.y = 0f; // Set vertical component to zero

        // Rotate towards the movement direction
        transform.rotation = Quaternion.LookRotation(movement);

        // Check if player is nearby
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Calculate the direction away from the player
            Vector3 awayFromPlayer = transform.position - player.position;
            awayFromPlayer.y = 0f; // Set vertical component to zero

            // Speed up and move away from the player
            movement = awayFromPlayer.normalized * currentMovementSpeed * speedMultiplier * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(awayFromPlayer); // Rotate towards the away direction
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, movement, out hit, movement.magnitude))
        {
            if (hit.collider.CompareTag("maze"))
            {
                // Change movement direction
                Vector3 newDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
                movement = newDirection.normalized * currentMovementSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(newDirection); // Rotate towards the new direction
            }
            else if (hit.collider.CompareTag("Player"))
            {
                // The rabbit was caught by the player
                CaughtByPlayer();
                return;
            }
        }

        transform.position += movement;
    }

    private void RestartMovement()
    {
        GenerateRandomMovementSpeed();
        isMoving = true;
    }

    private void GenerateRandomMovementSpeed()
    {
        currentMovementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }
}
