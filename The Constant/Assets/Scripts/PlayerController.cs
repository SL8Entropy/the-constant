using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerHealth playerHealth;

    [SerializeField] private int speed = 10;
    [SerializeField] private int jumpSpeed = 10;
    [SerializeField] private int gravityScale = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask clingLayer;
    [SerializeField] private LayerMask bashLayer;
    [SerializeField] private int bashSpeed;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isClinging = false;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    public bool isBashing { get; private set; } // Make this public for access
    private int clingDirection = 0;
    private float clingLayerIndex;
    private float bashLayerIndex;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 bashDirection;
    private float timeScale = 0.2f; // Time scale when bashing

    public Collider2D bashCol { get; private set; }

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        body = GetComponent<Rigidbody2D>();
        clingLayerIndex = Mathf.Log(clingLayer.value, 2f);
        bashLayerIndex = Mathf.Log(bashLayer.value, 2f);
    }

    private void Update()
    {
        if (isDashing)
        {
            HandleDash();
        }
        else
        {
            HandleMovement();
            HandleJump();
            HandleDashInput(); // Only handle dash input when not already dashing
        }
    }

    private void HandleMovement()
    {
        if (!playerHealth.immune && !isClinging)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0 && IsGrounded())
            {
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isClinging)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                if (horizontalInput * clingDirection < 0)
                {
                    body.velocity = new Vector2(horizontalInput * speed, jumpSpeed);
                    canDoubleJump = true;
                }
            }
            else if (IsGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpSpeed);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, jumpSpeed);
                canDoubleJump = false;
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (isBashing)
            {
                EndBash();
            }
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetButtonDown("Fire1") && dashCooldownTimer <= 0)
        {
            StartDash();
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void HandleDash()
    {
        dashTimeLeft -= Time.deltaTime;
        if (dashTimeLeft <= 0)
        {
            EndDash();
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;
        body.gravityScale = 0;
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * dashSpeed, 0);
    }

    private void EndDash()
    {
        isDashing = false;
        body.gravityScale = gravityScale;
    }

    private void EndBash()
    {
        isBashing = false;
        Time.timeScale = 1f;

        Vector2 closestPoint = bashCol.ClosestPoint((Vector2)bashCol.transform.position + bashDirection * 10f); // Convert Vector3 to Vector2
        body.MovePosition(closestPoint);
        body.velocity = bashDirection * bashSpeed;
        canDoubleJump = true;
        if(bashCol.gameObject.CompareTag("enemy")){
            // Get the EnemyGrunt component from the collided object
            EnemyClass enemy = bashCol.gameObject.GetComponent<EnemyClass>();

            // If the enemy component is found, call TakeDamage()
            if (enemy != null)
            {
                enemy.TakeDamage();
            }
        }
        else if(bashCol.gameObject.CompareTag("projectile")){
            Projectile projectile = bashCol.gameObject.GetComponent<Projectile>();
            projectile.rb.velocity = bashDirection*-1*projectile.projectileSpeed;
        }
    }

    private bool IsGrounded()
    {
        return body.IsTouchingLayers(groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == clingLayerIndex)
        {
            isClinging = true;
            body.velocity = Vector2.zero;
            body.gravityScale = 0;
            clingDirection = (collision.contacts[0].normal.x > 0.5f) ? -1 : 1;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == clingLayerIndex)
        {
            isClinging = false;
            body.gravityScale = gravityScale;
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (Input.GetButton("Fire2") && col.gameObject.layer == bashLayerIndex)
        {
            bashCol = col;
            Time.timeScale = timeScale;
            isBashing = true;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bashDirection = (mousePos - (Vector2)bashCol.transform.position).normalized;
        }
    }
}
