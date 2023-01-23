using Codice.Client.BaseCommands;
using DG.DemiEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static DialogueNodeData;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(200f, 200f);

    private List<string> copyCacheData;
    private List<DialogueNode> copyCacheDialogueNodes;

    public DialogueGraphView() {
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());

        serializeGraphElements += CutCopyOperation;
        unserializeAndPaste += PasteOperation;
        canPasteSerializedData += AllowPaste;
    }

    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            dialogueText = "ENTRYPOINT",
            entryPoint = true
        };

        var genratedPort = GeneratePort(node, Direction.Output);
        genratedPort.portName = "Next";
        node.outputContainer.Add(genratedPort);

        node.capabilities -= Capabilities.Movable;
        node.capabilities -= Capabilities.Deletable;
        node.capabilities -= Capabilities.Copiable;


        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100f, 200f, 100f, 150f));
        return node;
    }
    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    public DialogueNode CreateDialogueNode(string nodeName, ImageSignature state = ImageSignature.DEFAULT, int futureInt = 0)
    {
        var dialogueNode = new DialogueNode {

            dialogueText = nodeName,
            GUID = Guid.NewGuid().ToString(),
            state = state,
            title = state.ToString() + " Dialogue",
            futureInt = futureInt,
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.name= "Input";
        dialogueNode.inputContainer.Add(inputPort);

        //dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        var button = new Button(() => { AddChoicePort(dialogueNode); });
        button.text = "NewChoice";
        dialogueNode.titleContainer.Add(button);

        var dropDownMenu = new EnumField(ImageSignature.DEFAULT);
        dropDownMenu.value = dialogueNode.state;
        dropDownMenu.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.state = (ImageSignature)evt.newValue;
            dialogueNode.title = dialogueNode.state.ToString() + " Dialogue";
        });
        dialogueNode.inputContainer.Add(dropDownMenu);


        var gaugeValue = new IntegerField();
        gaugeValue.value = dialogueNode.futureInt;
        gaugeValue.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.futureInt = evt.newValue;
        });
        gaugeValue.SetValueWithoutNotify(dialogueNode.futureInt);
        dialogueNode.inputContainer.Add(gaugeValue);

        var textField = new TextField(string.Empty, -1, true, false, '*');
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.dialogueText = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.dialogueText);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
        dialogueNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

        return dialogueNode;
    }

    public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "" )
    {
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        var oldLabal = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabal);

        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName) ? $"Choice{outputPortCount + 1}" : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName,
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;

        dialogueNode.outputContainer.Add(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);

            RemoveElement(targetEdge.First());
        }

        dialogueNode.outputContainer.Remove(generatedPort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //return base.GetCompatiblePorts(startPort, nodeAdapter);

        var comptaiblePorts = new List<Port>();
        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                comptaiblePorts.Add(port);
            }
        });

        return comptaiblePorts;
    }

    private string CutCopyOperation(IEnumerable<GraphElement> elements)
    {
        foreach (DialogueNode node in elements.ToList())
        {
            DialogueNode copynode = CreateDialogueNode(node.dialogueText, node.state, node.futureInt);

            copynode.RefreshPorts();
            copynode.RefreshExpandedState();
            copynode.SetPosition(new Rect(node.GetPosition().position + new Vector2(50f, 50f), node.GetPosition().size));

            AddElement(copynode);
        }
        return string.Empty;
    }

    private void PasteOperation(string a, string b)
    {
        //Debug.Log("paste");
    }

    private bool AllowPaste(string data)
    {
        return true;
    }
}
