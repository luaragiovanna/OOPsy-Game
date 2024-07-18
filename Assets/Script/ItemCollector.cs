using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Logica de coleta do item
            Debug.Log("Item coletado: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
