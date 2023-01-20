using UnityEditor;
using UnityEditor.Experimental.GraphView;
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
        //GenerateMinimap();
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

        toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});
        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });


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

    private void GenerateMinimap()
    {
        var miniMap = new MiniMap
        {
            anchored = true,
        };
        miniMap.SetPosition(new Rect(10f, 30f, 100f, 70f));
        _graphView.Add(miniMap);
    }

}
