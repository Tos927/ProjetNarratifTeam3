using System;
using UnityEngine;

[Serializable]
public class DialogueNodeData 
{
    public string Guid;
    public string DialogueText;
    public ImageSignature State;
    public Vector2 Position;
    public int gaugeValue = 0;

    public enum ImageSignature
    {
        DEFAULT,
        GENERAL,
        BFF,
        FRERE,
    }
}
