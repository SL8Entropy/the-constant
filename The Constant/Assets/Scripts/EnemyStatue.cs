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
    private float xSize;

    public Vector2 shootDirection = Vector2.right; // The direction in which the statue can shoot

    void Update()
    {
        if (isDying) return; 

        HandleShooting();
        HandleHiding();
    }

    override public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        xSize = rb.transform.lossyScale.x;
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

    private void HandleHiding()
    {
        if (isHiding)
        {
            if (Time.time >= hideStartTime + hideDuration)
            {
                Unhide();
            }
        }
        else
        {
            if (Time.time >= hideStartTime + visibleDuration)
            {
                Hide();
            }
        }
    }

    private void Hide()
    {
        rb.transform.localScale = new Vector3(0, rb.transform.lossyScale.y, rb.transform.lossyScale.z);
        isHiding = true;
        hideStartTime = Time.time;
    }

    private void Unhide()
    {
        rb.transform.localScale = new Vector3(xSize, rb.transform.lossyScale.y, rb.transform.lossyScale.z);
        isHiding = false;
        hideStartTime = Time.time;
    }
    public override void TakeDamage()
    {
        enemyHealth+=(-1);
        if(enemyHealth<=0){
            Destroy(gameObject);
        }
    }
}
