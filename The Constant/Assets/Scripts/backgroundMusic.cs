using UnityEngine;

public class backgroundMusic:MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource.Play(); // Start playing music
    }
}
