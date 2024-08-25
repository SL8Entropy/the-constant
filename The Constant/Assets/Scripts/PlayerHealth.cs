using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int MaxPlayerHealth;
    [SerializeField] private int playerHealth;
    [SerializeField] private float knockbackJump; // Changed to float
    [SerializeField] private float maxImmunityTime; // Changed to float
    private float immunityTime;
    public bool immune = false;
    public GameObject[] hearts;
    private Player player;
    private Rigidbody2D body; // Declare body

    public GameObject gameOverScreen;
    public GameObject gameOverText;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        MaxPlayerHealth = hearts.Length;
        playerHealth = MaxPlayerHealth; // Initialize player health
        body = GetComponent<Rigidbody2D>();
        gameOverScreen.SetActive(false);
        gameOverText.SetActive(false);

    }

    public void changeHealth(int healthChange)
    {
        playerHealth += healthChange;
        if (playerHealth <= 0)
        {
            playerHealth = 0; // Ensure health does not go below 0
            GameOver();
        }

        // Disable all hearts
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }

        // Enable hearts according to current health
        for (int i = 0; i < playerHealth; i++)
        {
            hearts[i].SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!immune && (collision.gameObject.CompareTag("enemy") || collision.gameObject.CompareTag("projectile")))
        {
            changeHealth(-1);

            // Adjust knockback direction based on position
            if (collision.transform.position.x < body.position.x)
            {
                body.velocity = new Vector2(knockbackJump, knockbackJump);
            }
            else
            {
                body.velocity = new Vector2(-knockbackJump, knockbackJump);
            }

            immune = true;
            immunityTime = 0; // Reset immunity time when starting immunity
        }
    }

    private void FixedUpdate() // Corrected method signature
    {
        if (immune == true) // Corrected comparison operator
        {
            immunityTime += Time.deltaTime;
            if (immunityTime >= maxImmunityTime)
            {
                immune = false;
                immunityTime = 0; // Reset immunity time when immunity ends
            }
        }
    }

private void GameOver()
{
    player.enabled = false;  
    gameOverScreen.SetActive(true); 
    gameOverText.SetActive(true); 
}

}
