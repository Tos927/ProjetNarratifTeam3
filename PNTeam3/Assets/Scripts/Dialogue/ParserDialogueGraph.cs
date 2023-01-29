using System;
using System.Collections;
using System.Collections.Generic;
using T3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ParserDialogueGraph : MonoBehaviour
{
    [SerializeField] private DialogueContainer dialogueContainer;
    [SerializeField] private TextManager textmanager;
    [SerializeField] private GameObject decoder;
    [SerializeField] private GameObject consequences;
    [SerializeField] private Text mainText;
    [SerializeField] private Text signatureText;
    [SerializeField] private Image charaImage;
    [SerializeField] private List<Button> buttonList;
    //[SerializeField] private AudioMixerGroup mixer;

    //[SerializeField] private DialogueContainer dialogueContainer;
    private DialogueNodeData currentNode;
    private List<string> currentTextsChoices = new List<string>();

    private AudioSource audioSource;

    public int bffGauge = 0;
    public int frereGauge = 0;
    public bool showConsequences = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //currentNode = dialogueContainer.dialogueNodeDatas[0];
        for (int i = 0; i < dialogueContainer.dialogueNodeDatas.Count; i++)
        {
            if (dialogueContainer.dialogueNodeDatas[i].Guid == dialogueContainer.nodeLinks.Find(x => x.PortName == "Next").TargetNodeGuid)
            {
                currentNode = dialogueContainer.dialogueNodeDatas[i];
                charaImage.sprite = Resources.Load<Sprite>("Character/General");
                signatureText.font = Resources.Load<Font>("Font/Typewriter-Black");
                signatureText.text = "General Patin";

                audioSource.clip = currentNode.audioSource;
                Debug.Log(audioSource);
                //audioSource.outputAudioMixerGroup = mixer;
                //audioSource.Play();
                //Debug.Log(dialogueContainer.nodeLinks.Find(x => x.PortName == "Next").PortName);
            }
        }

    }

    public void GoToNextDialogue(Text port)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        string targetGuid = string.Empty;
        List<NodeLinkData> nodes = dialogueContainer.nodeLinks.FindAll(x => x.PortName == port.text);

        for (int i = 0; i < nodes.Count; i++)
        {
            Debug.Log(nodes[i].PortName);
            if (currentNode.Guid == nodes[i].baseNodeGuid)
            {
                targetGuid = nodes[i].TargetNodeGuid;
                //currentNode = dialogueContainer.dialogueNodeDatas[i];
            }
        }

        if (currentNode.consequense)
        {
            showConsequences = true;
            consequences.SetActive(true);
        }
        else
        {
            consequences.SetActive(false);
            showConsequences = false;
        }


        for (int i = 0; i < dialogueContainer.dialogueNodeDatas.Count; i++)
        {
            if (dialogueContainer.dialogueNodeDatas[i].Guid == targetGuid)
            {
                currentNode = dialogueContainer.dialogueNodeDatas[i];
            }
        }

        audioSource.clip = currentNode.audioSource;

        currentTextsChoices.Clear();
        //ReadCurrentNode();
        if (!showConsequences)
        {
            textmanager.InitText(currentNode.cocoInt);
            textmanager.StartPuzzle();
            decoder.SetActive(true);
        }
        UpdateGaugesAndImageSignature(currentNode);
    }

    private void UpdateGaugesAndImageSignature(DialogueNodeData nodeData)
    {
        //Check la Gaugevalue pour le frere et le bff
        switch (nodeData.State)
        {
            case DialogueNodeData.ImageSignature.BFF:
                bffGauge += nodeData.gaugeValue;
                charaImage.sprite = Resources.Load<Sprite>("Character/BFF1");
                signatureText.font = Resources.Load<Font>("Font/Autography");
                signatureText.text = "J.P.";
                break;
            case DialogueNodeData.ImageSignature.FRERE:
                frereGauge += nodeData.gaugeValue;
                charaImage.sprite = Resources.Load<Sprite>("Character/Frere1");
                signatureText.font = Resources.Load<Font>("Font/Signatra");
                signatureText.text = "Charles";
                break;

            default:
                charaImage.sprite = Resources.Load<Sprite>("Character/General");
                signatureText.font = Resources.Load<Font>("Font/Typewriter-Black");
                signatureText.text = "General Patin";
                break;
        }
    }

    public void ReadCurrentNode()
    {
        audioSource.Play();

        mainText.text = currentNode.DialogueText;

        for (int i = 0; i < dialogueContainer.nodeLinks.Count; i++)
        {
            if (currentNode.Guid == dialogueContainer.nodeLinks[i].baseNodeGuid)
            {
                currentTextsChoices.Add(dialogueContainer.nodeLinks[i].PortName);
            }
        }

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].gameObject.SetActive(true);
        }

        int diff = buttonList.Count - currentTextsChoices.Count;

        while(diff > 0)
        {
            buttonList[buttonList.Count - diff].gameObject.SetActive(false);
            diff--;
        }

        for (int i = 0; i < currentTextsChoices.Count; i++)
        {
            buttonList[i].transform.GetChild(0).GetComponent<Text>().text = currentTextsChoices[i];
        }
    }
}
