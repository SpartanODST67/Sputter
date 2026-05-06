public class DialogueRunner : MonoBehaviour
{
    public static DialogueRunner Instance { private set; get; }
    protected Dictionary<string, DialogueNode> dialogueMap;
    protected DialogueNode nextNode;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(string dialogueFile, string startingNode)
    {
        string json;
        try
        {
            json = File.ReadAllText($"Assets/Dialogue/{dialogueFile}.json");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }

        if (json == "")
        {
            Debug.LogError($"Failed to read file from ../../{dialogueFile}");
            return;
        }

        Debug.Log(json);

        dialogueMap = JsonConvert.DeserializeObject<Dictionary<string, DialogueNode>>(json);

        if (dialogueMap.Count == 0)
        {
            Debug.LogError($"Dialogue nodes are empty. Is {dialogueFile} empty?");
            return;
        }

        if (!dialogueMap.ContainsKey(startingNode))
        {
            Debug.LogError($"Dialogue does not contain {startingNode}");
            return;
        }

        RunDialogue(dialogueMap[startingNode]);
    }

    public void RunNode(DialogueNode targetNode)
    {
        RunDialogue(targetNode);
    }

    private void RunDialogue(DialogueNode currentNode)
    {
        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        nextNode = null;
        if (dialogueMap.TryGetValue(currentNode.nextDialogueNode, out DialogueNode foundNextNode))
        {
            nextNode = foundNextNode;
        }

        DialogueOption[] dialogueOptions = currentNode.options;

        DialogueBox.Instance.ShowDialogueBox(currentNode.speaker, currentNode.dialogue);

        if (dialogueOptions?.Length > 0)
        {
            DialogueNode[] targetNodes = new DialogueNode[dialogueOptions.Length];
            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                if (
                    dialogueMap.TryGetValue(
                        dialogueOptions[i].nextDialogueNode,
                        out DialogueNode foundNextBranch
                    )
                )
                {
                    targetNodes[i] = foundNextBranch;
                }
                else
                {
                    targetNodes[i] = null;
                }
            }

            DialogueBox.Instance.SetDialogueOptions(dialogueOptions, targetNodes);
        }
        else
        {
            DialogueBox.Instance.SetNextButton(nextNode);
        }
    }

    private void EndDialogue()
    {
        DialogueBox.Instance.HideDialogueBox();
    }
}
