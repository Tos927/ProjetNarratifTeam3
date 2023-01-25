using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueNode : Node
{
    public string GUID;
    public string dialogueText;
    public DialogueNodeData.ImageSignature state;
    public bool entryPoint = false;
    public int gaugeValue = 0;
}
