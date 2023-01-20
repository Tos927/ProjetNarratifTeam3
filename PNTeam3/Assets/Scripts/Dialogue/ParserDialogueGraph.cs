using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParserDialogueGraph : MonoBehaviour
{

    [SerializeField] private DialogueContainer dialogueContainer;
    [SerializeField] private Text mainText;
    [SerializeField] private List<Button> buttonList;

    //[SerializeField] private DialogueContainer dialogueContainer;
    private DialogueNodeData currentNode;
    private List<string> currentTextsChoices = new List<string>();


    private void Start()
    {
        //currentNode = dialogueContainer.dialogueNodeDatas[0];
        for (int i = 0; i < dialogueContainer.dialogueNodeDatas.Count; i++)
        {
            if (dialogueContainer.dialogueNodeDatas[i].Guid == dialogueContainer.nodeLinks.Find(x => x.PortName == "Next").TargetNodeGuid)
            {
                currentNode = dialogueContainer.dialogueNodeDatas[i];
                Debug.Log(dialogueContainer.nodeLinks.Find(x => x.PortName == "Next").PortName);
            }
        }
    }

    public void GoToNextDialogue(Text port)
    {
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

        for (int i = 0; i < dialogueContainer.dialogueNodeDatas.Count; i++)
        {
            if (dialogueContainer.dialogueNodeDatas[i].Guid == targetGuid)
            {
                currentNode = dialogueContainer.dialogueNodeDatas[i];
            }
        }

        currentTextsChoices.Clear();
        ReadCurrentNode();
    }

    public void ReadCurrentNode()
    {
        mainText.text = currentNode.DialogueText;

        for (int i = 0; i < dialogueContainer.nodeLinks.Count; i++)
        {
            if (currentNode.Guid == dialogueContainer.nodeLinks[i].baseNodeGuid)
            {
                currentTextsChoices.Add(dialogueContainer.nodeLinks[i].PortName);
                //Debug.Log(dialogueContainer.nodeLinks[i].PortName);
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
