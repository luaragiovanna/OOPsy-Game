using UnityEngine;

public class AmbienceAudio : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or AudioClip is missing on AmbienceAudio GameObject.");
        }
    }
}
