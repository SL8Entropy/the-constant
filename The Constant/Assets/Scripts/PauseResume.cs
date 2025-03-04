using UnityEngine;

public class PauseResume : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenue;
    public GameObject pauseText;
    void Awake()
    {
        pauseMenue.SetActive(false);
        pauseText.SetActive(false);
    }
    void Update()
    {
        // Press "Escape" or "P" to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pauseMenue.SetActive(true);
            pauseText.SetActive(true);

            PauseGame();
        }
        else
        {
            pauseMenue.SetActive(false);
            pauseText.SetActive(false);

            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;  // Stops game time
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;  // Resumes game time
        Debug.Log("Game Resumed");
    }

    public void ExitGame(){
        Application.Quit();
        
        // Just to confirm in the editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}


