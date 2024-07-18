using UnityEngine;
using UnityEngine.UI;

public class AttributeButton : MonoBehaviour
{
    public string attributeType; // "color", "method", "methodValue"
    public string attributeValue;

    private InstantiationPanelController instantiationPanelController;

    void Start()
    {
        instantiationPanelController = FindObjectOfType<InstantiationPanelController>();
        if (instantiationPanelController == null)
        {
            Debug.LogError("InstantiationPanelController not found in the scene.");
        }

        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        switch (attributeType)
        {
            case "color":
                instantiationPanelController.SetColor(attributeValue);
                break;
            case "method":
                instantiationPanelController.SetMethod(attributeValue);
                break;
            case "methodValue":
                instantiationPanelController.SetMethodValue(attributeValue);
                break;
            default:
                Debug.LogError("Invalid attribute type.");
                break;
        }
        Destroy(gameObject); // Remove o botão após a coleta
    }
}
