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
    private bool isHiding = false; // Whether the statue is currently hiding

    public Vector2 shootDirection = Vector2.right; // The direction in which the statue can shoot

    void Update()
    {
        if (isDying) return; 

        HandleShooting();
    }

    override public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    private void HandleShooting()
    {
        Vector2 toPlayer = (player.transform.position - rb.transform.position).normalized;

        // Check if the player is in the correct direction and within range
        if (Vector2.Dot(toPlayer, shootDirection) > 0.7f && Mathf.Abs(rb.transform.position.x - player.transform.position.x) <= 10 && Time.time >= nextShootTime && !isHiding)
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Vector3 spawnPosition = rb.transform.position + new Vector3(shootDirection.x * 0.8f, shootDirection.y * 0.8f, 0.0f);
        projectileInstance = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.projectileDirection = shootDirection.normalized;
            projectileComponent.Awake();
        }

        nextShootTime = Time.time + shootCooldown;
    }
    public override void TakeDamage()
    {
        enemyHealth-=1;
        if(enemyHealth<=0){
            Destroy(gameObject);
        }
    }
}
