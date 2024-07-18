using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectDialogue : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialogueBackground;
    public GameObject hintPanel; // Painel de dica
    public Text hintText; // Texto da dica

    private Queue<string> sentences;
    private bool hintShown = false; // Flag para garantir que o painel de dicas apare�a apenas uma vez inicialmente
    private bool secondHintShown = false; // Flag para garantir que a segunda dica seja exibida apenas uma vez

    private List<string> hints = new List<string>
    {
        "Dica: Movimente-se! � s� clicar no ch�o",
        "Dica: Clique no Alien"
    };

    void Start()
    {
        sentences = new Queue<string>();

        if (dialogueText == null)
        {
            Debug.LogError("Dialogue Text is not assigned in the Inspector");
        }

        if (dialogueBackground == null)
        {
            Debug.LogError("Dialogue Background is not assigned in the Inspector");
        }

        if (hintPanel != null)
        {
            hintPanel.SetActive(false); // Certifique-se de que o painel de dica est� desativado no in�cio
        }
    }

    public void StartDialogue(string[] dialogues)
    {
        if (dialogueText == null || dialogueBackground == null)
        {
            Debug.LogError("Dialogue Text or Background is not assigned");
            return;
        }

        dialogueBackground.SetActive(true);
        sentences.Clear();

        foreach (string sentence in dialogues)
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(DisplaySentences());
    }

    private IEnumerator DisplaySentences()
    {
        while (sentences.Count > 0)
        {
            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
            yield return new WaitForSeconds(1.5f); // Tempo de exibi��o de cada fala
        }

        EndDialogue();
    }

    public void EndDialogue()
    {
        dialogueBackground.SetActive(false);
        dialogueText.text = "";

        if (!hintShown && hintPanel != null)
        {
            hintShown = true;
            StartCoroutine(ShowHintPanel(0)); // Exibe a primeira dica
        }
    }

    private IEnumerator ShowHintPanel(int hintIndex)
    {
        hintText.text = hints[hintIndex];
        hintPanel.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        hintPanel.SetActive(false);

        if (hintIndex == 0 && !secondHintShown)
        {
            secondHintShown = true;
            yield return new WaitForSeconds(5f); // Espera 5 segundos antes de mostrar a segunda dica
            StartCoroutine(ShowHintPanel(1)); // Exibe a segunda dica
        }
    }

    // M�todo para ser chamado quando o jogador se move
    public void PlayerMoved()
    {
        if (!secondHintShown)
        {
            Debug.Log("Player moved, second hint will now be shown.");
            StartCoroutine(ShowHintPanel(1));
        }
    }

    // M�todo para iniciar um di�logo espec�fico
    public void ShowRocketDialogue()
    {
        string[] rocketDialogues = { "N�o posso voltar ainda", "tenho que catalogar material para analise" };
        StartDialogue(rocketDialogues);
    }
}
