using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDasher : EnemyClass
{
    private bool isPlayerMoving = false;
    private bool isDashing = false;
    private Vector2 lastPlayerPosition;
    private float idleMoveSpeed = 1.5f;
    private float dashSpeed = 6f;
    private float pauseDuration = 1.5f;
    private float moveDirection = 1;
    private bool isPaused = false;

    void Start()
    {
        lastPlayerPosition = player.transform.position;
        StartCoroutine(IdleMovement());
    }

    void Update()
    {
        if (!isDashing && !isPaused)
        {
            Vector2 currentPlayerPosition = player.transform.position;
            isPlayerMoving = (currentPlayerPosition != lastPlayerPosition);
            lastPlayerPosition = currentPlayerPosition;

            if (isPlayerMoving)
            {
                StopCoroutine(IdleMovement());
                StartCoroutine(ChaseAndDash());
            }
        }
    }

    private IEnumerator IdleMovement()
    {
        while (true)
        {
            rb.linearVelocity = new Vector2(idleMoveSpeed * moveDirection, rb.linearVelocity.y);
            yield return new WaitForSeconds(1);
            moveDirection *= -1; // Change direction
        }
    }

    private IEnumerator ChaseAndDash()
    {
        isPaused = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(pauseDuration);
        
        isPaused = false;
        isDashing = true;
        Vector2 dashDirection = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(0.5f);
        
        isDashing = false;
        StartCoroutine(IdleMovement());
    }

}
