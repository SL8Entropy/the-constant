using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private int speed = 10;
    [SerializeField] private int acceleration = 10;
    [SerializeField] private int jumpSpeed = 10;
    [SerializeField] private int gravityScale = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask clingLayer;
    [SerializeField] private LayerMask bashLayer;
    [SerializeField] private int bashSpeed;
    private bool isClinging;
    private bool canDoubleJump;
    private float clingLayerIndex;
    private float bashLayerIndex;
    private int clingDirection;
    public bool isBashing { get; private set; }
    public Collider2D bashCol { get; private set; }

    private Vector2 bashDirection; // Direction of the bash
    private float timeScale = 0.2f; //the time scale you want to slow down time by when you bash

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        clingLayerIndex = Mathf.Log(clingLayer.value, 2f);
        bashLayerIndex = Mathf.Log(bashLayer.value, 2f);
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleBash();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && !isClinging && IsGrounded())
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Time.timeScale = 1;
            if (isBashing)
            {
                isBashing = false;

                // Teleport to the bashable object
                body.MovePosition(bashCol.transform.position);

                // Launch in the chosen direction
                body.velocity = bashDirection * bashSpeed;
            }
        }

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
    }

    private void HandleBash()
    {
        if (Input.GetButton("Fire1") && bashCol != null)
        {
            Time.timeScale = timeScale;
            isBashing = true;

            // Calculate direction from player to mouse cursor
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bashDirection = (mousePos - (Vector2)bashCol.transform.position).normalized;
        }
    }

    private bool IsGrounded()
    {
        return body.IsTouchingLayers(groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var layerMask = collision.gameObject.layer;
        if (layerMask == clingLayerIndex)
        {
            isClinging = true;
            body.velocity = new Vector2(0, 0);
            body.gravityScale = 0;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 collisionNormal = contact.normal;

                if (collisionNormal.x > 0.5f)
                {
                    clingDirection = -1;
                }
                else if (collisionNormal.x < -0.5f)
                {
                    clingDirection = 1;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        var layerMask = col.gameObject.layer;
        if (Input.GetButton("Fire1"))
        {
            if (layerMask == bashLayerIndex)
            {
                bashCol = col;
                Time.timeScale = timeScale;
                isBashing = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var layerMask = collision.gameObject.layer;
        if (layerMask == clingLayerIndex)
        {
            isClinging = false;
            body.gravityScale = gravityScale;
        }
    }
}
