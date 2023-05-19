using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int initialLives = 10;
    public float damageRadius = 3f;
    public float catchRadius = 5f;
    public float damageInterval = 15f;
    public float catchInterval = 15f;

    public int currentRabbits = 0;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rabbitsCaughtText;

    private int currentLives;
    private bool canTakeDamage = true;
    private bool canCatchRabbit = true;
    private int rabbitsCaught = 0;
    private bool isPlayerAlive = true;
    public int rabbitsToWin = 5;

    private void Start()
    {
        currentLives = initialLives;
        UpdateLivesText();
        rabbitsCaught = currentRabbits;
        UpdateRabbitsCaughtText();
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            if (canTakeDamage)
            {
                CheckBadGuyCollisions();
            }
            else if (canCatchRabbit)
            {
                CheckRabbitCollisions();
            }
        }
    }

    private void CheckBadGuyCollisions()
    {
        GameObject[] badGuys = GameObject.FindGameObjectsWithTag("badguy");

        foreach (GameObject badGuy in badGuys)
        {
            float distance = Vector3.Distance(transform.position, badGuy.transform.position);

            if (distance <= damageRadius)
            {
                TakeDamage();
                break;
            }
        }
    }

    private void CheckRabbitCollisions()
    {
        GameObject[] rabbits = GameObject.FindGameObjectsWithTag("Rabbit");
        foreach (GameObject rabbit in rabbits)
        {
            float distance = Vector3.Distance(transform.position, rabbit.transform.position);

            if (distance <= catchRadius)
            {
                CaughtRabbit();
                break;
            }
        }
    }

    private void CaughtRabbit()
    {
        rabbitsCaught++;
        canCatchRabbit = false;
        UpdateRabbitsCaughtText();

        if (rabbitsCaught >= rabbitsToWin)
        {
            Won();
        }
        else
        {
            Invoke("EnableCatch", catchInterval);
        }
    }

    private void TakeDamage()
    {
        if (!isPlayerAlive)
        {
            return; 
        }

        currentLives--;
        canTakeDamage = false;
        UpdateLivesText();

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            Invoke("EnableDamage", damageInterval);
        }
    }

    private void EnableDamage()
    {
        canTakeDamage = true;
    }

    private void EnableCatch()
    {
        canCatchRabbit = true;
    }

    private void Die()
    {
        isPlayerAlive = false;
        deathText.text = "Player has died!";
    }

    private void Won()
    {
        isPlayerAlive = false;
        deathText.text = "Player has won!";
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives;
    }

    private void UpdateRabbitsCaughtText()
    {
        rabbitsCaughtText.text = "Rabbits Caught: " + rabbitsCaught;
    }
}
