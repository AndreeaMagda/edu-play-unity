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
        string paragraph = paragraphs.Dequeue();

        //update the dialogue text
        dialogueText.text = paragraph;

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
}
