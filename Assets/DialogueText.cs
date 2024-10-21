using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Container")]

public class DialogueText : ScriptableObject
{
    public string speakername;

    [TextArea(3, 10)]
    public string[] paragraphs;
}
