using System.Collections;
using UnityEngine;

public abstract class EnemyClass : MonoBehaviour
{
    public int enemyHealth = 1;
    public int enemySpeed = 1;
    public GameObject player;
    public Rigidbody2D rb;
    public Vector2 enemyDirection;
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer
    public float flashDuration = 1.0f; // Duration for flashing before destruction
    public float flashSpeed = 0.1f; // Speed of flashing effect
    public bool isDying = false;
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("projectile"))
        {
            TakeDamage();
        }
    }

    public virtual void TakeDamage()
    {
        enemyHealth -= 1;

        if (enemyHealth <= 0 && !isDying)
        {
            isDying = true;
            StopAllCoroutines(); // Stop movement coroutines
            rb.linearVelocity = Vector2.zero; // Freeze movement
            StartCoroutine(FlashBeforeDestroy());
        }
    }

    public IEnumerator FlashBeforeDestroy()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f); // Slightly transparent

        while (elapsedTime < flashDuration)
        {
            spriteRenderer.color = transparentColor; // Transparent
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.color = originalColor; // Back to normal
            yield return new WaitForSeconds(flashSpeed);
            elapsedTime += flashSpeed * 2;
        }

        // Fade out
        /*
        for (float alpha = 1f; alpha > 0; alpha -= 0.05f)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }
*/
        Destroy(gameObject);
    }
}
