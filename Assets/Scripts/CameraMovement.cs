using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Import Scene Management to reload the scene

public class CameraMovement : MonoBehaviour
{
    public Transform player;  // Reference to the player (drag the player GameObject here in the Inspector)
    public float cameraSpeed = 2f;  // Speed at which the camera moves up
    public float offset = 1f;       // Offset to give the player some breathing room before game over

    void Update()
    {
        // Move the camera upwards at a constant speed
        transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);

        // Check if the camera is higher than the player by a certain offset (to trigger game over)
        if (transform.position.y - offset > player.position.y)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Restart the current scene (essentially resets the game)
        Debug.Log("Game Over! Restarting the level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the active scene
    }
}
