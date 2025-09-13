using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{
    public string speaker;
    public string dialogue;
    public string nextDialogueNode;
    public DialogueOption[] options;
}

public class DialogueOption
{
    public string dialogue;
    public string nextDialogueNode;
}
