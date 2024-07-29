using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool missionComplete = false;
    public GameObjectDialogue dialogueManager; // Referência ao objeto DialogueManager na cena

    public void CompleteMission()
    {
        missionComplete = true;
    }

    void OnMouseDown()
    {
        if (missionComplete)
        {
            Debug.Log("Missão completa, vamos para casa!");
            // Lógica para finalizar o jogo ou avançar para a próxima fase
        }
        else
        {
            Debug.Log("Não posso ir agora... Tenho que completar minha missão.");
            if (dialogueManager != null)
            {
                dialogueManager.ShowRocketDialogue();
            }
        }
    }
}
