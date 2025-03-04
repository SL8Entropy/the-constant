using UnityEngine;
using TMPro;

public class timerScript : MonoBehaviour
{
    public TMP_Text timerText; // UI TextMeshPro to display the timer
    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Start()
    {
        StartTimer(); // Start the timer when the game begins
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

}
