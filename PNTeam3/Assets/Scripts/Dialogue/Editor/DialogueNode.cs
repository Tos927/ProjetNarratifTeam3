using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using static DialogueNodeData;

public class DialogueNode : Node
{
    public string GUID;
    public string dialogueText;
    public DialogueNodeData.ImageSignature state;
    public bool entryPoint = false;
    public int gaugeValue = 0;
    public int cocoInt = 0;
    public AudioClip audioSource;
}
