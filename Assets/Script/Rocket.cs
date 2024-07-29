using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool missionComplete = false;
    public GameObjectDialogue dialogueManager; // Refer�ncia ao objeto DialogueManager na cena

    public void CompleteMission()
    {
        missionComplete = true;
    }

    void OnMouseDown()
    {
        if (missionComplete)
        {
            Debug.Log("Miss�o completa, vamos para casa!");
            // L�gica para finalizar o jogo ou avan�ar para a pr�xima fase
        }
        else
        {
            Debug.Log("N�o posso ir agora... Tenho que completar minha miss�o.");
            if (dialogueManager != null)
            {
                dialogueManager.ShowRocketDialogue();
            }
        }
    }
}
