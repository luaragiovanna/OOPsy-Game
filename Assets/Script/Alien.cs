using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{
    public GameObjectDialogue dialogueManager;
    public GameObject extraterrestrialPrefab;
    public GameObject infoPanel;
    public Text infoText;
    public Button blueButton;
    public Button greenButton;
    public Button redButton;
    public GameObject optionsPanel; // Painel de op��es de di�logo
    public Button option1Button;
    public Button option2Button;
    public Button methodButton; // Bot�o para acionar o m�todo
    public Button understoodButton; // Bot�o "Entendi"
    public GameObject classPanel; // Painel da classe
    public GameObject challengePanel;
    private GameObject instantiatedObject;
    private bool isTalking = false; // Flag para verificar se o alien est� falando
    private bool isFloating = true; // Estado de flutua��o do ET01
    private Camera cinemachineCamera;
    private Vector3 initialPosition; // Posi��o inicial do ET01
    private Vector3 groundPosition; // Posi��o no solo
    private ClassPanelController classPanelController; // Controlador do painel da classe
    private InstantiationPanelController instantiationPanelController;
    public ItemCollectionManager itemCollectionManager;
    public Animator animator;

  


    void Start()
    {
        GameObject cameraObject = GameObject.FindWithTag("CinemachineCamera");
        if (cameraObject != null)
        {
            cinemachineCamera = cameraObject.GetComponent<Camera>();
            if (cinemachineCamera == null)
            {
                Debug.LogError("No Camera component found on the object with tag 'CinemachineCamera'.");
            }
        }
        else
        {
            Debug.LogError("No object with tag 'CinemachineCamera' found.");
        }
        animator = GetComponent<Animator>();
        infoPanel.SetActive(false);
        optionsPanel.SetActive(false);
        methodButton.gameObject.SetActive(false);
        understoodButton.gameObject.SetActive(false);
        classPanel.SetActive(false);

        blueButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(false);
        redButton.gameObject.SetActive(false);

        blueButton.onClick.AddListener(() => ChangeColor(Color.blue, "Azul"));
        greenButton.onClick.AddListener(() => ChangeColor(Color.green, "Verde"));
        redButton.onClick.AddListener(() => ChangeColor(Color.red, "Vermelho"));

        option1Button.onClick.AddListener(() => OnAttributeResponse(0));
        option2Button.onClick.AddListener(() => OnAttributeResponse(1));
        methodButton.onClick.AddListener(() => ToggleFloat());
        understoodButton.onClick.AddListener(() => EndMethodPhase());

        classPanelController = classPanel.GetComponent<ClassPanelController>();
        if (classPanelController == null)
        {
            Debug.LogError("ClassPanelController not found on classPanel.");
        }

        // Certificar que o painel de instancia��o est� desativado no in�cio
        instantiationPanelController = challengePanel.GetComponent<InstantiationPanelController>();
        if (instantiationPanelController != null)
        {
            instantiationPanelController.HidePseudocode();
            challengePanel.SetActive(false); // Desativa o painel no in�cio
        }
        else
        {
            Debug.LogError("InstantiationPanelController not found on challengePanel.");
        }
    }

    public void OnMouseDown()
    {
        if (isTalking)
        {
            Debug.Log("Alien is still talking.");
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue manager reference is not set in the Alien script.");
            return;
        }

        StartCoroutine(ExplainAndCreateObject());
    }

    private IEnumerator ExplainAndCreateObject()
    {
        isTalking = true;

        string[] explanationDialogues = {
            "Ol�! Eu sou o guardi�o deste planeta.",
            "Este � o Plane OBJETOS!",
            "Deixe-me te mostrar como criamos objetos aqui.",
            "Um objeto tem atributos como cor e forma.",
            "Vou criar um objeto extraterrestre."
        };
        dialogueManager.StartDialogue(explanationDialogues);

        yield return new WaitForSeconds(2 * explanationDialogues.Length);

        string[] creationDialogues = {
            "Veja, este � o nosso objeto.",
        };
        dialogueManager.StartDialogue(creationDialogues);

        yield return new WaitForSeconds(2 * creationDialogues.Length);

        if (extraterrestrialPrefab == null)
        {
            Debug.LogError("Extraterrestrial prefab is not assigned.");
            isTalking = false;
            yield break;
        }

        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
        }

        if (cinemachineCamera != null)
        {
            Vector3 cameraForward = cinemachineCamera.transform.forward;
            Vector3 spawnPosition = cinemachineCamera.transform.position + cameraForward * 3.0f + new Vector3(0, -1, 0);
            instantiatedObject = Instantiate(extraterrestrialPrefab, spawnPosition, Quaternion.identity);
            instantiatedObject.SetActive(true);

            // Defina a posi��o inicial e a posi��o no solo
            initialPosition = instantiatedObject.transform.position;
            groundPosition = new Vector3(initialPosition.x, -2.07f, initialPosition.z);

            CustomObject customObject = instantiatedObject.GetComponent<CustomObject>();
            if (customObject != null)
            {
                customObject.SetAttributes("roxo", "circular", 1);
            }

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.targetObject = instantiatedObject.transform;
                panelController.offset = new Vector3(0, 2, 0);
                infoPanel.SetActive(true);
            }

            DisplayObjectInfo(instantiatedObject);
        }
        else
        {
            Debug.LogError("Cinemachine camera not found.");
            isTalking = false;
            yield break;
        }

        yield return new WaitForSeconds(3);

        string[] attributeDialogues = {
            "   Esse objeto, do tipo extraterrestre, se chama ET01! E ele tem atributos...",
            "   Voc� gostaria de saber mais sobre atributos?"
        };
        dialogueManager.StartDialogue(attributeDialogues);

        yield return new WaitForSeconds(3); // Tempo de exibi��o da pergunta

        ShowOptionsPanel(); // Mostrar op��es ap�s a pergunta
    }

    private void ShowOptionsPanel()
    {
        Debug.Log("Showing options panel.");
        dialogueManager.EndDialogue(); // Remove o texto de di�logo antes de mostrar os bot�es
        optionsPanel.SetActive(true);
    }

    public void OnAttributeResponse(int responseIndex)
    {
        optionsPanel.SetActive(false);
        string[] responses = {
            "Uau! Muito legal!",
            "Atributos? O que � isso?"
        };

        if (responseIndex >= 0 && responseIndex < responses.Length)
        {
            string response = responses[responseIndex];
            if (responseIndex == 0) // "Legal!"
            {
                string[] followUpDialogues = { response, "Concordo!" };
                dialogueManager.StartDialogue(followUpDialogues);
                StartCoroutine(FinalizeInteraction());
            }
            else if (responseIndex == 1) // "Atributos?"
            {
                string[] followUpDialogues = { response, "  Atributos s�o caracter�sticas..", "Que objetos de uma mesma classe compartilham!", "Vamos! agora experimente!", "Altere o atributo cor!" };
                dialogueManager.StartDialogue(followUpDialogues);
                StartCoroutine(ShowColorButtons());
            }
        }
    }

    private IEnumerator FinalizeInteraction()
    {
        yield return new WaitForSeconds(3); // Tempo para o di�logo "Concordo!"
        string[] finalDialogues = { "..." };
        dialogueManager.StartDialogue(finalDialogues);
        isTalking = false;
        StartCoroutine(ExplainMethod());
    }

    private IEnumerator ShowColorButtons()
    {
        yield return new WaitForSeconds(4); // Tempo para o di�logo "Vamos! agora experimente! Altere o atributo cor!"
        blueButton.gameObject.SetActive(true);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);
    }

    private void ChangeColor(Color color, string colorName)
    {
        if (instantiatedObject == null)
        {
            Debug.LogError("Instantiated object is null.");
            return;
        }

        SkinnedMeshRenderer renderer = instantiatedObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (renderer != null)
        {
            // Instanciar o material para garantir que estamos modificando a inst�ncia correta
            renderer.material = new Material(renderer.material);

            // Alterar a cor do Albedo com base no nome da cor
            switch (colorName.ToLower())
            {
                case "vermelho":
                    renderer.material.SetColor("_Color", new Color(1f, 0f, 0.011f)); // FF0003
                    break;
                case "verde":
                    renderer.material.SetColor("_Color", new Color(0.082f, 1f, 0f)); // 15FF00
                    break;
                case "azul":
                    renderer.material.SetColor("_Color", new Color(0.329f, 0.369f, 0.953f)); // 545EF3
                    break;
                default:
                    renderer.material.color = color;
                    break;
            }
            Debug.Log($"Material albedo color changed to {colorName}");

            CustomObject customObject = instantiatedObject.GetComponent<CustomObject>();
            if (customObject != null)
            {
                customObject.SetAttributes(colorName.ToLower(), customObject.objectShape, customObject.numDeOlhos);
                DisplayObjectInfo(instantiatedObject);
            }
        }
        else
        {
            Debug.LogError("No SkinnedMeshRenderer component found on the instantiated object or its children.");
        }

        blueButton.gameObject.SetActive(false);
        greenButton.gameObject.SetActive(false);
        redButton.gameObject.SetActive(false);

        string[] finalDialogues = {
        "   Agora, voc� alterou o atributo cor do extraterrestre!"
    };
        dialogueManager.StartDialogue(finalDialogues);
        isTalking = false; // Permitir intera��o ap�s a fala final
        StartCoroutine(ExplainMethod());
    }



    private IEnumerator ExplainMethod()
    {
        string[] methodDialogues = {
            "   Legal! Agora que voc� sabe o que atributos s�o, vamos para os m�todos!",
            "   Lembra deste objeto n�? O ET01! Consegue ver que agora ele tem um m�todo?",
            "   Consegue ver esse 'true'? Significa que o m�todo est� ativo!",
            "   Vamos! Experimente acionar o m�todo pra ver o que acontece!"
        };
        dialogueManager.StartDialogue(methodDialogues);

        yield return new WaitForSeconds(1 * methodDialogues.Length);

        methodButton.gameObject.SetActive(true);
        understoodButton.gameObject.SetActive(true);

        DisplayMethodInfo(instantiatedObject);
    }

    private void ToggleFloat()
    {
        isFloating = !isFloating;

        // Atualizar a posi��o do objeto com base no estado de flutua��o
        if (isFloating)
        {
            instantiatedObject.transform.position = initialPosition;
            animator.SetBool("isFloating", true);
        }
        else
        {
            instantiatedObject.transform.position = groundPosition;
            animator.SetBool("isFloating", false);
            StartCoroutine(PlayDieAnimation());
        }

        DisplayMethodInfo(instantiatedObject);
    }

    private IEnumerator PlayDieAnimation()
    {
        animator.Play("Die");
        yield return new WaitForSeconds(1.0f); // Tempo suficiente para alcan�ar o frame 20
        animator.speed = 0; // Pausa a anima��o no frame atual
    }

    private void DisplayMethodInfo(GameObject obj)
    {
        CustomObject customObject = obj.GetComponent<CustomObject>();
        if (customObject != null)
        {
            string objectInfo = $"ET01:Extraterrestre\n" +
                                $"modelo: {customObject.objectShape}\n" +
                                $"cor: {customObject.objectColor}\n" +
                                $"n�mero de olhos: {customObject.numDeOlhos}\n" +
                                $"flutuar: {isFloating}\n";

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.UpdateInfoText(objectInfo);
                panelController.SetStatic(); // Nova fun��o para tornar o painel est�tico
            }
        }
    }

    private void DisplayObjectInfo(GameObject obj)
    {
        CustomObject customObject = obj.GetComponent<CustomObject>();
        if (customObject != null)
        {
            string objectInfo = $"ET01:Extraterrestre\n" +
                                $"modelo: {customObject.objectShape}\n" +
                                $"cor: {customObject.objectColor}\n" +
                                $"n�mero de olhos: {customObject.numDeOlhos}\n";

            InfoPanelController panelController = infoPanel.GetComponent<InfoPanelController>();
            if (panelController != null)
            {
                panelController.UpdateInfoText(objectInfo);
                panelController.SetStatic(); // Nova fun��o para tornar o painel est�tico
            }
        }
    }

    private void EndMethodPhase()
    {
        methodButton.gameObject.SetActive(false);
        understoodButton.gameObject.SetActive(false);
        infoPanel.SetActive(false);
        if (instantiatedObject != null)
        {
            instantiatedObject.SetActive(false);
        }
        dialogueManager.EndDialogue();
        StartCoroutine(ExplainClasses());
    }

    private IEnumerator ExplainClasses()
    {
        string[] classDialogues = {
            "  Agora que sabemos a estrutura de um objeto...", "precisamos saber de onde ele vem...",
            "  Todos os seres deste planeta pertencem a mesma classe..",
            "  O nome da classe � PlanetaObjetos!"
        };
        dialogueManager.StartDialogue(classDialogues);

        yield return new WaitForSeconds(2 * classDialogues.Length);

        if (classPanelController != null)
        {
            string classInfo = "Extraterrestre\n- cor: string\n- modelo: string\n- numOlhos: int\n+ flutuar(): boolean";
            classPanelController.ShowClassInfo(classInfo);
            Debug.Log("Class panel activated and showing info.");
        }
        else
        {
            Debug.LogError("ClassPanelController is not assigned.");
        }

        yield return new WaitForSeconds(3);

        string[] playerQuestion = {
            "Ent�o voc� tamb�m � um objeto???"
        };
        dialogueManager.StartDialogue(playerQuestion);

        yield return new WaitForSeconds(3);

        string[] alienResponse = {
            "Sim! Assim como ET01 sou um objeto desta classe",
            "Isso significa que posso usar o m�todo flutuar!"
        };
        dialogueManager.StartDialogue(alienResponse);

        yield return new WaitForSeconds(2 * alienResponse.Length);

        // L�gica para fazer o alien flutuar por 3 segundos e depois voltar ao ch�o
        Vector3 alienInitialPosition = transform.position;
        Vector3 floatingPosition = new Vector3(alienInitialPosition.x, -1.16f, alienInitialPosition.z);

        transform.position = floatingPosition;
        yield return new WaitForSeconds(3);
        transform.position = alienInitialPosition;

        if (classPanelController != null)
        {
            classPanelController.HideClassInfo(); // Esconder o painel da classe ap�s a demonstra��o
        }
        ShowChallengePanel();
        // Mostrar o painel de instancia��o ap�s a fala "Veja! Um novo ser est� sendo inst�nciado"
        string[] newObjectDialogue = {
            //deve aparecer o painelChallenge antes de falar:
            "Veja! Um novo ser est� sendo inst�nciado",
            "Mas est�o faltando partes dele!"
        };
        dialogueManager.StartDialogue(newObjectDialogue);

        yield return new WaitForSeconds(2 * newObjectDialogue.Length);

        InstantiationPanelController instantiationPanelController = FindObjectOfType<InstantiationPanelController>();
        if (instantiationPanelController != null)
        {
            instantiationPanelController.ShowPseudocode("ser = novo Extraterrestre()\nser.cor = [ ??? ]\nser.numOlhos = 1\nser.modelo = circular\n---------M�todos---------\nser.[ ]();");
        }

        yield return new WaitForSeconds(3);

        string[] searchItemsDialogue = {
            "Procure o valor do atributo cor, o m�todo e seu par�metro"
        };
        dialogueManager.StartDialogue(searchItemsDialogue);

        yield return new WaitForSeconds(2 * searchItemsDialogue.Length);

        // Esconder o painel de instancia��o ap�s a fala "Procure o valor do atributo cor, o m�todo e seu par�metro"
        if (instantiationPanelController != null)
        {
            instantiationPanelController.HidePseudocode();
        }
    }

    private void ShowChallengePanel()
    {
        if (instantiationPanelController != null)
        {
            Debug.Log("Showing challenge panel.");
            challengePanel.SetActive(true); // Ativa o painel quando necess�rio
            instantiationPanelController.ShowPseudocode("ser = novo Extraterrestre()\nser.cor = [ ??? ]\nser.numOlhos = 1\nser.modelo = circular\n---------M�todos---------\nser.[ ]();");
        }
        else
        {
            Debug.LogError("InstantiationPanelController not found on challengePanel.");
        }
    }
    
}