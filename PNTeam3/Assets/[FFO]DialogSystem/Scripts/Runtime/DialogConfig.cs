using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogConfig : MonoBehaviour
{
    [System.Serializable]
    public struct SpeakerConfig
    {

        public SpeakerDatabase speakerDatabase;
        public SpeakerData speakerData;

    }

    public List<SpeakerConfig> speakers = new();

    [System.Serializable]
    public struct SentenceConfig
    {
        [TextArea]public string sentence;
        public AudioClip audioClip;
    }

    public List<SpeakerDatabase> speakerDatabases = new();

    [Header("SENTENCES")]
    public List<SentenceConfig> sentenceConfig = new();
}
