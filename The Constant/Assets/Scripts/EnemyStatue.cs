using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatue : EnemyClass
{
    public GameObject projectilePrefab; // Prefab of the projectile
    private GameObject projectileInstance; // Instance of the projectile
    public float shootCooldown = 5.0f; // Cooldown between shots
    private float nextShootTime = 0f; // Time until next shot can be fired
    public float hideDuration = 2.5f; // Duration for which the statue hides
    public float visibleDuration = 2.5f; // Duration for which the statue is visible
    private float hideStartTime = 0f; // Time when the statue starts hiding
    private bool isHiding = false; // Whether the statue is currently hiding

    void Update()
    {
        // Check if the enemy should shoot a projectile
        HandleShooting();

        // Check if the enemy should hide or come out of hiding
        HandleHiding();
    }

    // Method to handle shooting logic
    private void HandleShooting()
    {
        // Check if the player is within range and the cooldown period has passed
        if (Mathf.Abs(rb.transform.position.x - player.transform.position.x) <= 10 && Time.time >= nextShootTime && !isHiding)
        {
            ShootProjectile();
        }
    }

    // Method to shoot a projectile
    private void ShootProjectile()
    {
        // Calculate the spawn position for the projectile
        Vector3 spawnPosition = rb.transform.position + new Vector3(0.8f, 0.0f, 0.0f);

        // Instantiate the projectile at the new position
        projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        // Set the projectile's direction and speed
        Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.projectileDirection = new Vector2(1, 0).normalized;
            projectileComponent.Awake();
        }

        // Set the next shoot time based on the cooldown
        nextShootTime = Time.time + shootCooldown;
    }

    // Method to handle hiding logic
    private void HandleHiding()
    {
        if (isHiding)
        {
            // Check if the hide duration has elapsed
            if (Time.time >= hideStartTime + hideDuration)
            {
                // Unhide the enemy
                Unhide();
            }
        }
        else
        {
            // Check if the visible duration has elapsed
            if (Time.time >= hideStartTime + visibleDuration)
            {
                // Hide the enemy
                Hide();
            }
        }
    }

    // Method to hide the enemy
    private void Hide()
    {
        rb.transform.localScale = new Vector3(rb.transform.localScale.x, 0, rb.transform.localScale.z);
        isHiding = true;
        hideStartTime = Time.time; // Start hiding timer
    }

    // Method to unhide the enemy
    private void Unhide()
    {
        rb.transform.localScale = new Vector3(rb.transform.localScale.x, 0.8f, rb.transform.localScale.z);
        isHiding = false;
        hideStartTime = Time.time; // Start visible timer
    }
}
