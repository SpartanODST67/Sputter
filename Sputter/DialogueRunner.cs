using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueRunner : MonoBehaviour
{
    public static DialogueRunner Instance { private set; get; }
    protected Dictionary<string, DialogueNode> dialogueMap;

    private void Awake()
    {
        Instance = this;
    }

    public void RunDialogue(string dialogueFile, string startingNode)
    {
        string json = "";
        try
        {
            json = File.ReadAllText($"Assets/Dialogue/{dialogueFile}.json");
        } catch (Exception e)
        {
            Debug.LogError( e.Message );
            return;
        }

        if (json == "")
        {
            Debug.LogError($"Failed to read file from ../../{dialogueFile}");
            return;
        }

        Debug.Log(json);

        dialogueMap = JsonConvert.DeserializeObject<Dictionary<string, DialogueNode>>(json);
        
        if(dialogueMap.Count == 0)
        {
            Debug.LogError($"Dialogue nodes are empty. Is {dialogueFile} empty?");
            return;
        }

        foreach(var elem in dialogueMap)
        {
            Debug.Log($"{elem.Key}: {elem.Value.dialogue}");
        }

        if(!dialogueMap.ContainsKey(startingNode))
        {
            Debug.LogError($"Dialogue does not contain {startingNode}");
            return;
        }

        DialogueNode startNode = dialogueMap[startingNode];

        DialogueBox.Instance.ShowText(startNode.speaker, startNode.dialogue);
    }
}
