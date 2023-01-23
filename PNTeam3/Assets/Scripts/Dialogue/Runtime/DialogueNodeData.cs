using System;
using UnityEngine;

[Serializable]
public class DialogueNodeData 
{

    public string Guid;
    public string DialogueText;
    public ImageSignature State;
    //public string statestrin;
    public Vector2 Position;

    public enum ImageSignature
    {
        DEFAULT,
        GENERAL,
        BFF,
        BFF_HURT,
        BFF_MIDLY_HURT,
        BFF_BADLY_HURT,
        FRERE,
        FRERE_HURT,
        FRERE_MIDLY_HURT,
        FRERE_BADLY_HURT
    }
}
