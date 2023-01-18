using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInitDialogue : MonoBehaviour
{
    public TextAsset csv;
    private SentencesParserSystem parser;

    // Start is called before the first frame update
    void Start()
    {
        parser = new SentencesParserSystem();
        parser.Load(csv);
        //GetComponent<DialogConfig>().sentenceConfig = parser.GetRowList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
