using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    public int initialLives = 10;
    public float damageRadius = 3f;
    public float catchRadius = 5f;
    public float damageInterval = 15f;
    public float catchInterval = 15f;
    public float rabbitHideDuration = 20f;
    public float nextLifeCooldown = 10f;

    public int currentRabbits = 0;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI triggerText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI rabbitsCaughtText;

    private int currentLives;
    private bool canTakeDamage = true;
    private bool isPlayerAlive = true;
    private int rabbitsCaught = 0;
    private int rabbitsToWin = 5;

    private void Start()
    {
        currentLives = initialLives;
        UpdateLivesText();
        rabbitsCaught = currentRabbits;
        UpdateRabbitsCaughtText();
        triggerText.text = "Catch all rabbits, avoid the bugs! Press E to catch a rabbit!";
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            if (canTakeDamage)
            {
                CheckBadGuyCollisions();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckRabbitCollisions();
        }

        if (rabbitsCaught >= rabbitsToWin)
        {
            Won();
        }

        if (currentLives <= 0)
        {
            Die();
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, catchRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Rabbit"))
            {
                CatchRabbit(collider.gameObject);
                break;
            }
        }
    }

    private void CatchRabbit(GameObject rabbit)
    {
        float distance = Vector3.Distance(transform.position, rabbit.transform.position);

        if (distance <= catchRadius)
        {
            rabbit.SetActive(false);
            rabbitsCaught++;
            UpdateRabbitsCaughtText();

            StartCoroutine(ShowRabbitAfterDelay(rabbit));
        }
    }

    private IEnumerator ShowRabbitAfterDelay(GameObject rabbit)
    {
        yield return new WaitForSeconds(rabbitHideDuration);

        rabbit.SetActive(true);
    }

    private void TakeDamage()
    {
        if (!isPlayerAlive)
        {
            return;
        }

        currentLives--;
        UpdateLivesText();

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            canTakeDamage = false;
            Invoke("EnableDamage", nextLifeCooldown);
        }
    }

    private void EnableDamage()
    {
        canTakeDamage = true;
    }

    private void Die()
    {
        isPlayerAlive = false;
        deathText.text = "Player has died!";
        SceneManager.LoadScene("DIED");
    }

    private void Won()
    {
        isPlayerAlive = false;
        deathText.text = "Player has won!";
        SceneManager.LoadScene("WON");
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
