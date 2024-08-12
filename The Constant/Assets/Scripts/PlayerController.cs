using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private int speed = 10;
    [SerializeField] private int jumpSpeed = 10;
    [SerializeField] private int gravityScale = 3;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask clingLayer;
    private bool isClinging;
    private bool canDoubleJump;
    private float clingLayerIndex;
    private int clingDirection;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        clingLayerIndex = Mathf.Log(clingLayer.value, 2f);
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && !isClinging)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
    }

    private void HandleJump()
    {
        
        

        if (Input.GetButtonDown("Jump"))
        {
            if(isClinging){
                float horizontalInput = Input.GetAxis("Horizontal");
                if(horizontalInput * clingDirection <=0 ){
                    body.velocity = new Vector2(horizontalInput*speed,jumpSpeed);
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
                body.velocity = new Vector2(body.velocity.x, jumpSpeed);
                canDoubleJump = false;
            }
        }
        
    }

    private bool IsGrounded()
    {
        // Checks if the player's collider is touching any collider on the groundLayer
        return body.IsTouchingLayers(groundLayer);
    }
    private void OnCollisionEnter2D ( Collision2D collision )
    {
        var layerMask = collision.gameObject.layer;
        if (layerMask == clingLayerIndex){
            isClinging = true;
            body.velocity=new Vector2 (0,0);
            body.gravityScale=0;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // The contact.normal gives the direction of the collision
                Vector2 collisionNormal = contact.normal;

                // Use the collision normal to determine specific directions
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


    private void OnCollisionExit2D(Collision2D collision){
        var layerMask = collision.gameObject.layer;
        if(layerMask == clingLayerIndex){
            isClinging = false;
            body.gravityScale = gravityScale;
        }
    }
    /*
    private Vector2 GetClingingDirection()
    {
        if (RaycastInDirection(Vector2.left, clingLayer)) return Vector2.left;
        if (RaycastInDirection(Vector2.right, clingLayer)) return Vector2.right;

        return Vector2.zero; // Not touching any direction
    }
    private bool RaycastInDirection(Vector2 direction, LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, layer);
        return hit.collider != null;
    }
    */
}