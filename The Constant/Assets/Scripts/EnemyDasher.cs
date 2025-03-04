using System.Collections;
using UnityEngine;

public class EnemyDasher : EnemyClass
{
    private bool isDashing = false;
    private Vector2 lastPlayerPosition;
    private float idleMoveSpeed = 1.5f;
    private float dashSpeed = 6f;
    private float pauseDuration = 1.5f;
    private float moveDirection = 1;
    private bool isPaused = false;
    private Coroutine chaseCoroutine = null; // Store coroutine reference

    void Start()
    {
        lastPlayerPosition = player.transform.position;
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (isDying) return;  // Stop movement if dying

        if (!isDashing && !isPaused && chaseCoroutine == null)
        {
            chaseCoroutine = StartCoroutine(ChaseAndDash());
        }
    }

    private IEnumerator IdleMovement()
    {
        while (!isDying) // Stop moving when dying
        {
            rb.linearVelocity = new Vector2(idleMoveSpeed * moveDirection, rb.linearVelocity.y);
            yield return new WaitForSeconds(1);
            moveDirection *= -1; // Change direction
        }
        rb.linearVelocity = Vector2.zero; // Ensure stopping
    }

    private IEnumerator ChaseAndDash()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(pauseDuration);

        if (isDying) yield break; // Stop execution if dying

        isPaused = false;
        isDashing = true;

        Vector2 dashDirection = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(0.5f);

        isDashing = false;
        chaseCoroutine = null; // Reset coroutine reference

        if (!isDying) StartCoroutine(IdleMovement()); // Resume idle movement if alive
    }

}
