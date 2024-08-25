using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrunt : EnemyClass
{
    public GameObject projectilePrefab; // Prefab of the projectile
    private GameObject projectileInstance; // Instance of the projectile
    private float shootCooldown = 2.5f; // Cooldown between shots
    private float nextShootTime = 0f; // Time until next shot can be fired

    // Update is called once per frame
    void Update()
    {
        // Move enemy towards the player with a slight offset
        enemyDirection = new Vector2(player.transform.position.x - rb.transform.position.x, 5 + player.transform.position.y - rb.transform.position.y);
        rb.velocity = enemyDirection.normalized * enemySpeed;

        // Check if the enemy is within firing range and cooldown has elapsed
        if (Mathf.Abs(rb.transform.position.x - player.transform.position.x) <= 5 && Time.time >= nextShootTime)
        {
            // Calculate the new position where the projectile will be instantiated
            Vector3 spawnPosition = rb.transform.position + new Vector3(0.0f, -2f, 0.0f);

            // Instantiate the projectile at the new position
            projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Get the Projectile component and set its direction and speed
            Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.projectileDirection = new Vector2(0, -1).normalized;
                projectileComponent.Awake();
            }

            // Set the next available shoot time
            nextShootTime = Time.time + shootCooldown;
        }
    }
}
