using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 projectileDirection; // Direction of the projectile
    public float projectileSpeed; // Speed of the projectile

    public Rigidbody2D rb;

    // Awake is called when the script instance is being loaded
    public void Awake()
    {
        // Get the Rigidbody2D component attached to the projectile
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this GameObject.");
            return;
        }

        // Ensure the projectileDirection is normalized (has a magnitude of 1)
        projectileDirection.Normalize();

        // Set gravity scale to 0 to prevent gravity from affecting the projectile
        rb.gravityScale = 0;

        // Set the velocity of the Rigidbody2D
        rb.velocity = projectileDirection * projectileSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision){
        Destroy(gameObject);
    }
}
