using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    void LateUpdate()
    {
        if (!target)
            return;

        // Calcula a posi��o desejada da c�mera
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Suaviza a rota��o
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Suaviza a altura
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Converte a rota��o em um quaternion
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Define a posi��o da c�mera
        transform.position = target.position - currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Sempre olha para o alvo
        transform.LookAt(target);
    }
}
