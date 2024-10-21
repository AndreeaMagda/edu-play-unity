using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionsManager : MonoBehaviour
{
    [SerializeField] private DialogueText text;
    [SerializeField] private DialogueManager manager;

    public void Talk(DialogueText text)
    {
        //start the conversation
        manager.DisplayNextDialogue(text);
    }
}
