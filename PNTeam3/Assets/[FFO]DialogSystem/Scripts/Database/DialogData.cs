using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogData
{
    public string id;
    public string label;
    public NewSentenceConfigPlus sentence = new();
    public AnswersSentences[] answers = new AnswersSentences[4];

    [System.Serializable]
    public struct NewSentenceConfigPlus
    {
        public AudioClip audioClip;
        [TextArea] public string sentence;
    }
    [System.Serializable]
    public struct AnswersSentences
    {
        public int value;
        [TextArea] public string answerSentence;
    }
}
