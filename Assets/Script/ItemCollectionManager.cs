using UnityEngine;

public class ItemCollectionManager : MonoBehaviour
{
    public InstantiationPanelController instantiationPanelController;
    public int totalItemsToCollect = 3;

    private string collectedColor = "[ ??? ]";
    private string collectedMethod = "[ ???]";
    private string collectedMethodValue = "[ ??? ]";
    private int collectedItemCount = 0;

    public void ItemCollected(CollectibleItem.ItemType itemType, string value)
    {
        switch (itemType)
        {
            case CollectibleItem.ItemType.Color:
                collectedColor = value;
                break;
            case CollectibleItem.ItemType.Method:
                collectedMethod = value;
                break;
            case CollectibleItem.ItemType.MethodValue:
                collectedMethodValue = value;
                break;
        }

        UpdateChallengePanel();

        collectedItemCount++;
        if (collectedItemCount == totalItemsToCollect)
        {
            ShowChallengePanel(); // Mostra o painel do desafio
            instantiationPanelController.EnableInstantiateButton(); // Ativa o botão de instanciação
        }
    }

    private void UpdateChallengePanel()
    {
        if (instantiationPanelController != null)
        {
            string updatedPseudocode = $"ser = novo Extraterrestre()\nser.cor = {collectedColor}\nser.numOlhos = 1\nser.modelo = circular\n---------Métodos---------\nser.{collectedMethod}({collectedMethodValue});";
            instantiationPanelController.ShowPseudocode(updatedPseudocode);
        }
    }

    private void ShowChallengePanel()
    {
        if (instantiationPanelController != null)
        {
            Debug.Log("Showing challenge panel.");
            instantiationPanelController.gameObject.SetActive(true); // Ativa o painel quando necessário
            UpdateChallengePanel();
        }
        else
        {
            Debug.LogError("InstantiationPanelController not found on challengePanel.");
        }
    }
}
