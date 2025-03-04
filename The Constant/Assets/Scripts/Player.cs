using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerHealth playerHealth;

    [SerializeField] private int speed = 10;
    [SerializeField] private float bashProjectileSpeedMod = 2;
    [SerializeField] private int jumpSpeed = 10;
    [SerializeField] private int gravityScale = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask clingLayer;
    [SerializeField] private LayerMask bashLayer;
    [SerializeField] private int bashSpeed;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float bashCooldown = 1.5f; // Cooldown duration for bash

    private bool isClinging = false;
    private bool canDoubleJump = false;
    private bool isDashing = false;
    public bool isBashing { get; private set; }
    private int clingDirection = 0;
    private float clingLayerIndex;
    private float bashLayerIndex;
    private float dashTimeLeft = 0f;
    private float dashCooldownTimer = 0f;
    private float bashCooldownTimer = 0f;
    private Vector2 bashDirection;
    private float timeScale = 0.1f;

    public Collider2D bashCol { get; private set; }

    private void Awake()
    {
        playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
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
            if (!playerHealth.immune)
            {
                HandleMovement();
                HandleJump();
                HandleDashInput();
            }
        }

        HandleBashCooldown();
    }

    private void HandleMovement()
    {
        if (!playerHealth.immune && !isClinging)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            if (horizontalInput != 0 && IsGrounded())
            {
                body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
            }
        }
        if(body.position.y<=-10){
            playerHealth.changeHealth(-1);
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
                    body.linearVelocity = new Vector2(horizontalInput * speed, jumpSpeed);
                    canDoubleJump = true;
                }
            }
            else if (IsGrounded())
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, jumpSpeed);
                canDoubleJump = false;
            }
        }

        if (Input.GetButtonUp("Fire2") && isBashing)
        {
            EndBash(1);
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
        body.linearVelocity = new Vector2(horizontalInput * dashSpeed, 0);
    }

    private void EndDash()
    {
        isDashing = false;
        body.gravityScale = gravityScale;
    }

    public void EndBash(int x)
    {
        isBashing = false;
        Time.timeScale = 1f;
        if (x == 1)
        {
            Vector2 closestPoint = bashCol.ClosestPoint((Vector2)bashCol.transform.position + bashDirection * 10f);
            body.MovePosition(closestPoint);
            body.linearVelocity = bashDirection * bashSpeed;
            canDoubleJump = true;

            if (bashCol.gameObject.CompareTag("enemy"))
            {
                EnemyClass enemy = bashCol.gameObject.GetComponent<EnemyClass>();
                if (enemy != null)
                {
                    enemy.TakeDamage();
                }
            }
            else if (bashCol.gameObject.CompareTag("projectile"))
            {
                Projectile projectile = bashCol.gameObject.GetComponent<Projectile>();
                projectile.rb.linearVelocity = bashDirection * -1 * projectile.projectileSpeed * bashProjectileSpeedMod;
            }
        }


        bashCooldownTimer = bashCooldown; // Start cooldown after bash ends
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
            body.linearVelocity = Vector2.zero;
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
        if (Input.GetButton("Fire2") && col.gameObject.layer == bashLayerIndex && bashCooldownTimer <= 0 && !playerHealth.immune)
        {
            bashCol = col;
            Time.timeScale = timeScale;
            isBashing = true;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bashDirection = (mousePos - (Vector2)bashCol.transform.position).normalized;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == bashLayerIndex)
        {
            EndBash(0);
        }
    }

    private void HandleBashCooldown()
    {
        if (bashCooldownTimer > 0)
        {
            bashCooldownTimer -= Time.deltaTime;
        }
    }
}
