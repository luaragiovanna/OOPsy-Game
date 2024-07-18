using UnityEngine;
using UnityEngine.UI;

public class ClassPanelController : MonoBehaviour
{
    public Text classInfoText;

    public void ShowClassInfo(string classInfo)
    {
        Debug.Log("ShowClassInfo called.");
        if (classInfoText != null)
        {
            classInfoText.text = classInfo;
            gameObject.SetActive(true);
            Debug.Log("Class info updated and panel activated.");
        }
        else
        {
            Debug.LogError("ClassInfoText is not assigned.");
        }
    }

    public void HideClassInfo()
    {
        gameObject.SetActive(false); // Método para esconder o painel
    }
}
