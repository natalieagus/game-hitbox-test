using UnityEngine;

public class Geralt : NPC, ITalkable
{
    [SerializeField] private DialogueText dialogueText;
    [SerializeField] private DialogueController controller;
    private bool isFirstLine = true;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        if (isFirstLine)
        {
            // playsound
            audioSource.Play();
            isFirstLine = false;
        } 
        bool conversationEnded =  controller.DisplayNextParagraph(dialogueText);
        if (conversationEnded)
        {
            isFirstLine = true;
        }
    }
}
