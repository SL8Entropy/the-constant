using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    public int enemyHealth = 1;
    public int enemySpeed = 1;
    public GameObject player;
    public Rigidbody2D rb;
    public Vector2 enemyDirection;
    // Start is called before the first frame update
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
    }

    public void TakeDamage()
    {
        // Reduce health by 1
        enemyHealth -= 1;

        // Check if health is 0 or less
        if (enemyHealth <= 0)
        {
            // Destroy the game object if health is depleted
            Destroy(gameObject);
        }
    }
}
