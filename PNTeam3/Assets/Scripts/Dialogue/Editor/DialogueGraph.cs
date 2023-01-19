using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenGraphWindow()
    {
        var windows = GetWindow<DialogueGraph>();
        windows.titleContent = new GUIContent("Dialogue Graph");

    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }



    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolBar()
    {
        var toolbar = new Toolbar();

        var fileNametextField = new TextField("File Name :");
        fileNametextField.SetValueWithoutNotify(_fileName);
        fileNametextField.MarkDirtyRepaint();
        fileNametextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNametextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)){text = "Save Data"});
        toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});

        var nodeCreateButton = new Button(() => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);

    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name", "Enter a valid file name", "OK");
        }

        var saveUtility = GraphSaveUtility.instance(_graphView);

        if (save)
            saveUtility.SaveGraph(_fileName);
        else 
            saveUtility.LoadGraph(_fileName);
    }
}
