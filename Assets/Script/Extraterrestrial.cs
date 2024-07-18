using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraterrestrial : MonoBehaviour
{
    public AudioClip interactionSound; 
    private AudioSource audioSource;

    void Start()
    {
        // Adicione um componente AudioSource se não houver um
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verifique se interactionSound foi atribuído no Inspetor
        if (interactionSound == null)
        {
            Debug.LogError("Interaction sound is not assigned.");
            return; // Sai do Start se o interactionSound não estiver atribuído
        }

        audioSource.clip = interactionSound;
        audioSource.playOnAwake = false; // Garante que o som não será tocado automaticamente
    }

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown called on Extraterrestrial.");

        // Tocar o som ao clicar no objeto
        if (audioSource != null && interactionSound != null)
        {
            audioSource.Play();
            Debug.Log("Extraterrestrial clicked and sound played.");
        }
        else
        {
            Debug.LogError("AudioSource or InteractionSound is not assigned.");
        }
    }
}
