using Codice.Client.BaseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
    public static GraphSaveUtility instance(DialogueGraphView targetGraphView) {

        return new GraphSaveUtility
        {

            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string filename)
    {
        if (!Edges.Any()) return;

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        var connectedPort = Edges.Where(x => x.input.node != null).ToArray();
        for (int i = 0; i < connectedPort.Length; i++)
        {
            var outputNode = connectedPort[i].output.node as DialogueNode;
            var inputNode = connectedPort[i].input.node as DialogueNode;

            dialogueContainer.nodeLinks.Add(new NodeLinkData
            {
                baseNodeGuid = outputNode.GUID,
                PortName = connectedPort[i].output.portName,
                TargetNodeGuid= inputNode.GUID,
            }); 
        }

        foreach (var dialogueNode in nodes.Where(Node => !Node.entryPoint))
        {
            dialogueContainer.dialogueNodeDatas.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.dialogueText,
                State = dialogueNode.state,
                Position = dialogueNode.GetPosition().position,
                gaugeValue = dialogueNode.gaugeValue,
                audioSource = dialogueNode.audioSource,
            });
        }
        //Autocreate folder
        if (!AssetDatabase.IsValidFolder($"Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{filename}.asset");
        AssetDatabase.SaveAssets();
    }   
    public void LoadGraph(string filename)
    {
        _containerCache =  Resources.Load<DialogueContainer>(filename);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph does not exists!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ClearGraph()
    {
        nodes.Find(x => x.entryPoint).GUID = _containerCache.nodeLinks[0].baseNodeGuid;

        foreach (var node in nodes)
        {
            if (node.entryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }

    private void CreateNodes()
    {
        foreach (var nodeData in _containerCache.dialogueNodeDatas)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, nodeData.State, nodeData.gaugeValue, nodeData.audioSource);
            tempNode.GUID = nodeData.Guid;

            _targetGraphView.AddElement(tempNode);
            var nodePorts = _containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x=> _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }
    private void ConnectNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var connections = _containerCache.nodeLinks.Where(x => x.baseNodeGuid == nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                linkeNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                targetNode.SetPosition(new Rect(_containerCache.dialogueNodeDatas.First(x=>x.Guid == targetNodeGuid).Position, _targetGraphView.defaultNodeSize));
            }
        }
    }

    private void linkeNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input,
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }
}
