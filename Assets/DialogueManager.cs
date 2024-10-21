using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 10f;

    private Queue<string> paragraphs = new Queue<string>();

    private bool ConvoEnded, isTyping;
    private Coroutine typeRoutine;

    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_SPEED = 0.1f;

    public void DisplayNextDialogue(DialogueText dialogue)
    {
        //if the queue is empty
        if (paragraphs.Count == 0)
        {
            if (!ConvoEnded)
            {
                //start the new conversation
                StartConversation(dialogue);
            }
            else
            {
                //end the conversation
                EndConversation();
                return;
            }
        }

        //if there is something in queue
        if (!isTyping)
        {
            string paragraph = paragraphs.Dequeue();

            typeRoutine = StartCoroutine(TypeDialogueText(paragraph));
        }
        //this types the text all at once
        //dialogueText.text = paragraph;

        if(paragraphs.Count == 0)
        {
            ConvoEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogue)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        //update speaker name
        speakerName.text = dialogue.speakername;

        for (int i = 0; i < dialogue.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogue.paragraphs[i]);
        }
    }

    private void EndConversation()
    {
        paragraphs.Clear(); //clear the queue

        ConvoEnded= false;
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        dialogueText.text = "";

        string original = p;
        string displayed = "";
        int alpha = 0;

        foreach (char c in p.ToCharArray())
        {
            alpha++;
            dialogueText.text = original;

            displayed = dialogueText.text.Insert(alpha, HTML_ALPHA);
            dialogueText.text = displayed;

            yield return new WaitForSeconds(MAX_TYPE_SPEED / typingSpeed);
        }

        isTyping = false;
    }
}
