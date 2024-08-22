using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashArrow : MonoBehaviour
{
    public GameObject arrowPrefab; // The arrow prefab to instantiate
    private GameObject arrowInstance;
    private Player player;

    private void Start()
    {
        // Find the Player script in the scene
        player = FindObjectOfType<Player>();

        // Instantiate the arrow and set it to inactive initially
        arrowInstance = Instantiate(arrowPrefab);
        arrowInstance.SetActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.isBashing)
            {
                // Activate the arrow
                arrowInstance.SetActive(true);

                // Position the arrow at the bashable object's position
                arrowInstance.transform.position = player.bashCol.transform.position;

                // Calculate the direction from the bashable object to the mouse position
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = (mousePos - (Vector2)player.bashCol.transform.position).normalized;

                // Rotate the arrow to point towards the mouse position
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                // Hide the arrow when not bashing
                arrowInstance.SetActive(false);
            }
        }
    }
}
