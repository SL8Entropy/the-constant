using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector2 offset;    // Offset from the player position
    public Vector2 boundary;  // Boundary in world units around the camera center

    private Vector2 cameraPosition; // Camera position in Vector2 for easier calculations

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned to CameraFollow script.");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Get the camera's current position
            cameraPosition = transform.position;

            // Get the player's position
            Vector2 playerPosition = player.position;

            // Calculate the difference between the camera and player positions
            Vector2 difference = playerPosition - cameraPosition;

            // Calculate new camera position based on boundaries
            if (difference.x > boundary.x)
            {
                cameraPosition.x = playerPosition.x - boundary.x;
            }
            else if (difference.x < -boundary.x)
            {
                cameraPosition.x = playerPosition.x + boundary.x;
            }

            if (difference.y > boundary.y)
            {
                cameraPosition.y = playerPosition.y - boundary.y;
            }
            else if (difference.y < -boundary.y)
            {
                cameraPosition.y = playerPosition.y + boundary.y;
            }

            // Apply the offset and set the new camera position
            transform.position = new Vector3(cameraPosition.x + offset.x, cameraPosition.y + offset.y, transform.position.z);
        }
    }
}
