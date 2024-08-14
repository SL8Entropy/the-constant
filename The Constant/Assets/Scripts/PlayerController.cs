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
    private bool isBashing;
    private Collider2D bashCol;
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
        
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && !isClinging && IsGrounded())
        {
            /*
            body.AddForce(new Vector2(horizontalInput*acceleration, 0));
            if(Mathf.Abs(body.velocity.x) > speed){
                body.velocity = new Vector2 (speed*horizontalInput, body.velocity.y);
            }*/
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
    }

    private void HandleJump()
    {
        
        if(Input.GetButtonUp("Fire1")){
            Time.timeScale = 1;
            if(isBashing){
                isBashing = false;
                // body.move position to edge of the bashCol in direction of mousepos-bashColpos
                body.MovePosition((Vector2)bashCol.transform.position);
                //body.velocity = vector2 of mouse pos - body pos.normalalised*speed*2
                body.velocity = new Vector2(0,0);
                body.AddForce(new Vector2(bashSpeed,bashSpeed), ForceMode2D.Impulse);
            }
            
            
        }

        if (Input.GetButtonDown("Jump"))
        {
            if(isClinging){
                float horizontalInput = Input.GetAxis("Horizontal");
                if(horizontalInput * clingDirection <0 ){
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
                body.velocity = new Vector2(Input.GetAxis("Horizontal")*speed, jumpSpeed);
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
    private void OnTriggerStay2D(Collider2D col){
        var layerMask = col.gameObject.layer;
        if (Input.GetButton("Fire1")){
            if(layerMask == bashLayerIndex){
                bashCol = col;
                Time.timeScale = timeScale;
                isBashing = true;
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