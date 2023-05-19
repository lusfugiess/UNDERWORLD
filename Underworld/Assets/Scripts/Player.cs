using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int initialLives = 10;
    public float damageRadius = 3f;
    public float damageInterval = 15f;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI deathText;

    private int currentLives;
    private bool canTakeDamage = true;

    private void Start()
    {
        currentLives = initialLives;
        UpdateLivesText();
    }

    private void Update()
    {
        if (canTakeDamage)
        {
            CheckBadGuyCollisions();
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

    private void TakeDamage()
    {
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

    private void Die()
    {
        deathText.text = "Player has died!";
        // Implement your game over logic here
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives;
    }
}