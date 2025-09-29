using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueController: MonoBehaviour
{
    [SerializeField] private float textSpeed = 100f;
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;

    private Queue<string> parapraphs = new Queue<string>();
    private bool conversationEnded;
    private string currParagraph;
    private bool isTyping;

    private Coroutine typeDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";

    // Returns true if conversation has finished
    public bool DisplayNextParagraph(DialogueText dialogueText)
    {
        if (parapraphs.Count == 0) 
        { 
            if (!conversationEnded)
            {
                StartConversation(dialogueText);
            }
            else if(conversationEnded && !isTyping)
            {
                EndConversation();
                return true;
            }
        }

        if (!isTyping)
        {
            currParagraph = parapraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(currParagraph));
        }
        else
        {
            FinishParagraphEarly(currParagraph);
        }
        if (parapraphs.Count == 0)
        {
            conversationEnded = true;
        }
        return false;
    }

    public void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        NPCNameText.text = dialogueText.speakerName;

        for (int i = 0; i < dialogueText.paragraphs.Length; i++) {
            parapraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    private void EndConversation()
    {
        parapraphs.Clear();
        conversationEnded = false;
        if (gameObject.activeSelf)
        {
            gameObject.SetActive (false);
        }
    }

    private IEnumerator TypeDialogueText(string currParagraph)
    {
        isTyping = true;
        NPCDialogueText.text = "";

        string originalText = currParagraph;
        string displayedText = "";
        int alphaIndex = 0;
        foreach (char c in currParagraph.ToCharArray()) {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogueText.text = displayedText;
            yield return new WaitForSeconds(1 / textSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly(string currParagraph)
    {
        StopCoroutine(typeDialogueCoroutine);
        NPCDialogueText.text = currParagraph;
        isTyping = false;
    }
}
