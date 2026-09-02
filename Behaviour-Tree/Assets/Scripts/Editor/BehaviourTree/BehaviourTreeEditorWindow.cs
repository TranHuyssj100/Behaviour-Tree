using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BehaviourTreeEditorWindow : EditorWindow
{
    const string RootPath = "Root";
    const string ChildMarker = ".children.Array.data[";

    BehaviourTreeSO asset;
    SerializedObject serializedObject;
    string selectedPath = RootPath;
    Vector2 treeScroll;
    Vector2 inspectorScroll;
    readonly Dictionary<string, bool> foldouts = new();

    [MenuItem("Window/AI/Behaviour Tree Editor")]
    public static void OpenEmpty()
    {
        GetWindow<BehaviourTreeEditorWindow>("Behaviour Tree");
    }

    public static void Open(BehaviourTreeSO so)
    {
        var window = GetWindow<BehaviourTreeEditorWindow>("Behaviour Tree");
        window.Bind(so);
        window.Focus();
    }

    [UnityEditor.Callbacks.OnOpenAsset]
    static bool OnOpenAsset(int instanceId, int line)
    {
        if (EditorUtility.InstanceIDToObject(instanceId) is not BehaviourTreeSO so)
            return false;
        Open(so);
        return true;
    }

    void OnSelectionChange()
    {
        if (Selection.activeObject is BehaviourTreeSO so)
            Bind(so);
        Repaint();
    }

    void OnEnable()
    {
        if (asset == null && Selection.activeObject is BehaviourTreeSO so)
            Bind(so);
    }

    void Bind(BehaviourTreeSO so)
    {
        asset = so;
        serializedObject = so != null ? new SerializedObject(so) : null;
        selectedPath = RootPath;
        foldouts.Clear();
        foldouts[RootPath] = true;
    }

    void OnGUI()
    {
        if (asset == null || serializedObject == null)
        {
            EditorGUILayout.HelpBox("Chọn một asset BehaviourTreeSO trên Project, hoặc mở asset đó.", MessageType.Info);
            return;
        }

        serializedObject.Update();
        DrawToolbar();

        EditorGUILayout.BeginHorizontal();
        DrawTreePanel();
        DrawInspectorPanel();
        EditorGUILayout.EndHorizontal();

        HandleKeyboard();
        serializedObject.ApplyModifiedProperties();
    }

    void DrawToolbar()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label(asset.name, EditorStyles.boldLabel, GUILayout.Width(180));
        if (GUILayout.Button("Ping", EditorStyles.toolbarButton, GUILayout.Width(50)))
            EditorGUIUtility.PingObject(asset);

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Tạo cây Customer (Selector)", EditorStyles.toolbarButton, GUILayout.Width(210)))
        {
            if (asset.Root == null || EditorUtility.DisplayDialog(
                    "Tạo cây mẫu",
                    "Ghi đè Root hiện tại bằng Selector: GotoBuyPizza + GotoMakeMoney?",
                    "Tạo", "Hủy"))
            {
                CreateCustomerTemplate();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    void DrawTreePanel()
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(Mathf.Max(280, position.width * 0.48f)));
        GUILayout.Label("Cây hành vi", EditorStyles.boldLabel);

        treeScroll = EditorGUILayout.BeginScrollView(treeScroll, GUI.skin.box);

        var rootProp = serializedObject.FindProperty(RootPath);
        if (rootProp == null || rootProp.managedReferenceValue == null)
            DrawEmptyRoot();
        else
            DrawNodeRow(rootProp, 0);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawEmptyRoot()
    {
        EditorGUILayout.HelpBox("Root đang trống. Chọn loại node gốc.", MessageType.Warning);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Selector (OR)", GUILayout.Height(28)))
            SetRoot(typeof(SelectorNode), "Customer");
        if (GUILayout.Button("Sequence (AND)", GUILayout.Height(28)))
            SetRoot(typeof(SequenceNode), "Sequence");
        EditorGUILayout.EndHorizontal();

        DrawAddTypeMenu("Hoặc chọn type khác…", SetRootFromMenu);
    }

    void DrawNodeRow(SerializedProperty nodeProp, int depth)
    {
        if (nodeProp == null) return;

        var node = nodeProp.managedReferenceValue as BTNode;
        if (node == null)
        {
            EditorGUILayout.LabelField(new string(' ', depth * 2) + "(null)");
            return;
        }

        var path = nodeProp.propertyPath;
        bool isSelected = selectedPath == path;
        bool isComposite = node is CompositeNode;
        bool expanded = GetFoldout(path);

        var row = EditorGUILayout.BeginHorizontal();
        if (isSelected && Event.current.type == EventType.Repaint)
            EditorGUI.DrawRect(row, new Color(0.24f, 0.43f, 0.68f, 0.45f));

        GUILayout.Space(depth * 16);

        var color = TypeColor(node.GetType());
        var colorRect = GUILayoutUtility.GetRect(6, 18, GUILayout.Width(6));
        EditorGUI.DrawRect(colorRect, color);

        if (isComposite)
        {
            expanded = EditorGUILayout.Foldout(expanded, GUIContent.none, true);
            SetFoldout(path, expanded);
        }
        else
        {
            GUILayout.Space(16);
        }

        string label = $"{node.GetDisplayName()}  ({BTNodeTypes.Category(node.GetType())})";
        if (GUILayout.Button(label, isSelected ? EditorStyles.whiteLabel : EditorStyles.label))
            selectedPath = path;

        EditorGUILayout.EndHorizontal();

        var lastRect = GUILayoutUtility.GetLastRect();
        HandleRowEvents(lastRect, nodeProp, isComposite);

        if (isComposite && expanded)
        {
            var children = nodeProp.FindPropertyRelative("children");
            if (children != null)
            {
                for (int i = 0; i < children.arraySize; i++)
                    DrawNodeRow(children.GetArrayElementAtIndex(i), depth + 1);
            }
        }
    }

    void HandleRowEvents(Rect row, SerializedProperty nodeProp, bool isComposite)
    {
        var e = Event.current;
        if (!row.Contains(e.mousePosition)) return;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            selectedPath = nodeProp.propertyPath;
            e.Use();
            Repaint();
        }
        else if (e.type == EventType.ContextClick)
        {
            selectedPath = nodeProp.propertyPath;
            ShowNodeContextMenu(nodeProp, isComposite);
            e.Use();
        }
    }

    void DrawInspectorPanel()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Node được chọn", EditorStyles.boldLabel);

        inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll, GUI.skin.box);

        var nodeProp = serializedObject.FindProperty(selectedPath);
        if (nodeProp == null || nodeProp.managedReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Chưa chọn node.", MessageType.Info);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            return;
        }

        var node = (BTNode)nodeProp.managedReferenceValue;
        var type = node.GetType();

        EditorGUILayout.LabelField("Type", type.Name);
        EditorGUILayout.LabelField("Loại", BTNodeTypes.Category(type));

        var nameProp = nodeProp.FindPropertyRelative("nodeName");
        if (nameProp != null)
        {
            EditorGUILayout.PropertyField(nameProp, new GUIContent("Tên hiển thị"));
            if (string.IsNullOrWhiteSpace(nameProp.stringValue))
                EditorGUILayout.HelpBox("Trống sẽ hiện tên class. Ví dụ: GotoBuyPizza, GotoMakeMoney.", MessageType.None);
        }

        DrawExtraFields(nodeProp);

        EditorGUILayout.Space(8);

        if (node is CompositeNode)
        {
            GUILayout.Label("Thêm con", EditorStyles.boldLabel);
            DrawAddTypeMenu("Thêm BTNode vào node này", t => AddChild(nodeProp, t));
            EditorGUILayout.HelpBox(
                "Selector: thử từng con đến khi không Failure.\nSequence: chạy lần lượt, dừng khi không Success.",
                MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Condition / Action là node lá — không chứa con.", MessageType.None);
        }

        EditorGUILayout.Space(8);
        DrawHierarchyButtons(nodeProp);

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void DrawExtraFields(SerializedProperty nodeProp)
    {
        var iterator = nodeProp.Copy();
        var end = nodeProp.GetEndProperty();
        bool enterChildren = true;
        while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
        {
            enterChildren = false;
            if (iterator.name == "nodeName" || iterator.name == "children") continue;
            EditorGUILayout.PropertyField(iterator, true);
        }
    }

    void DrawHierarchyButtons(SerializedProperty nodeProp)
    {
        bool isRoot = nodeProp.propertyPath == RootPath;
        EditorGUILayout.BeginHorizontal();

        GUI.enabled = !isRoot;
        if (GUILayout.Button("Lên"))
            MoveSelected(-1);
        if (GUILayout.Button("Xuống"))
            MoveSelected(1);
        GUI.enabled = true;

        GUI.backgroundColor = new Color(1f, 0.55f, 0.55f);
        GUI.enabled = !isRoot;
        if (GUILayout.Button("Xóa node"))
            RemoveSelected();
        GUI.enabled = true;
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndHorizontal();

        if (isRoot)
            EditorGUILayout.HelpBox("Không xóa Root. Muốn đổi gốc: xóa cây mẫu rồi tạo Root mới, hoặc dùng template.", MessageType.None);
    }

    void DrawAddTypeMenu(string buttonLabel, Action<Type> onPick)
    {
        if (!GUILayout.Button(buttonLabel, GUILayout.Height(24))) return;

        var menu = new GenericMenu();
        foreach (var type in BTNodeTypes.All())
        {
            var captured = type;
            menu.AddItem(
                new GUIContent($"{BTNodeTypes.Category(type)}/{type.Name}"),
                false,
                () =>
                {
                    onPick(captured);
                    serializedObject.ApplyModifiedProperties();
                    Repaint();
                });
        }
        menu.ShowAsContext();
    }

    void ShowNodeContextMenu(SerializedProperty nodeProp, bool isComposite)
    {
        var menu = new GenericMenu();
        if (isComposite)
        {
            foreach (var type in BTNodeTypes.All())
            {
                var captured = type;
                menu.AddItem(
                    new GUIContent("Thêm con/" + BTNodeTypes.Category(type) + "/" + type.Name),
                    false,
                    () =>
                    {
                        AddChild(nodeProp, captured);
                        serializedObject.ApplyModifiedProperties();
                    });
            }
        }

        if (nodeProp.propertyPath != RootPath)
        {
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Xóa"), false, RemoveSelected);
        }

        menu.ShowAsContext();
    }

    void SetRootFromMenu(Type type) => SetRoot(type, null);

    void SetRoot(Type type, string name)
    {
        Undo.RecordObject(asset, "Set BT Root");
        var rootProp = serializedObject.FindProperty(RootPath);
        var instance = (BTNode)Activator.CreateInstance(type);
        instance.NodeName = name;
        rootProp.managedReferenceValue = instance;
        selectedPath = RootPath;
        SetFoldout(RootPath, true);
        EditorUtility.SetDirty(asset);
    }

    void AddChild(SerializedProperty parentProp, Type type)
    {
        if (parentProp.managedReferenceValue is not CompositeNode)
        {
            EditorUtility.DisplayDialog("Không thể thêm con", "Chỉ Selector / Sequence mới chứa BTNode con.", "OK");
            return;
        }

        Undo.RecordObject(asset, "Add BT Node");
        var children = parentProp.FindPropertyRelative("children");
        int index = children.arraySize;
        children.arraySize = index + 1;
        var element = children.GetArrayElementAtIndex(index);
        var instance = (BTNode)Activator.CreateInstance(type);
        element.managedReferenceValue = instance;
        selectedPath = element.propertyPath;
        SetFoldout(parentProp.propertyPath, true);
        EditorUtility.SetDirty(asset);
    }

    void RemoveSelected()
    {
        if (!TryGetParent(selectedPath, out string parentPath, out int index))
            return;

        Undo.RecordObject(asset, "Remove BT Node");
        var parentProp = serializedObject.FindProperty(parentPath);
        var children = parentProp.FindPropertyRelative("children");
        children.DeleteArrayElementAtIndex(index);
        selectedPath = parentPath;
        EditorUtility.SetDirty(asset);
    }

    void MoveSelected(int delta)
    {
        if (!TryGetParent(selectedPath, out string parentPath, out int index))
            return;

        var parentProp = serializedObject.FindProperty(parentPath);
        var children = parentProp.FindPropertyRelative("children");
        int target = index + delta;
        if (target < 0 || target >= children.arraySize) return;

        Undo.RecordObject(asset, "Reorder BT Node");
        children.MoveArrayElement(index, target);
        selectedPath = children.GetArrayElementAtIndex(target).propertyPath;
        EditorUtility.SetDirty(asset);
    }

    void HandleKeyboard()
    {
        var e = Event.current;
        if (e.type != EventType.KeyDown) return;
        if (e.keyCode == KeyCode.Delete)
        {
            RemoveSelected();
            e.Use();
        }
    }

    void CreateCustomerTemplate()
    {
        Undo.RecordObject(asset, "Create Customer BT");

        var buy = new SequenceNode { NodeName = "GotoBuyPizza" };
        buy.children.Add(new HasEnoughMoneyCondition());
        buy.children.Add(new MoveToShopAction());
        buy.children.Add(new BuyWantedItemAction());

        var earn = new SequenceNode { NodeName = "GotoMakeMoney" };
        earn.children.Add(new NotEnoughMoneyCondition());
        earn.children.Add(new WithdrawMoneyAction());

        var root = new SelectorNode { NodeName = "Customer" };
        root.children.Add(buy);
        root.children.Add(earn);

        serializedObject.FindProperty(RootPath).managedReferenceValue = root;
        selectedPath = RootPath;
        SetFoldout(RootPath, true);
        EditorUtility.SetDirty(asset);
        serializedObject.ApplyModifiedProperties();
    }

    bool GetFoldout(string path)
    {
        if (!foldouts.ContainsKey(path))
            foldouts[path] = true;
        return foldouts[path];
    }

    void SetFoldout(string path, bool value) => foldouts[path] = value;

    static bool TryGetParent(string path, out string parentPath, out int index)
    {
        int marker = path.LastIndexOf(ChildMarker, StringComparison.Ordinal);
        if (marker < 0)
        {
            parentPath = null;
            index = -1;
            return false;
        }

        parentPath = path.Substring(0, marker);
        int start = marker + ChildMarker.Length;
        int end = path.IndexOf(']', start);
        index = int.Parse(path.Substring(start, end - start));
        return true;
    }

    static Color TypeColor(Type t)
    {
        if (typeof(SelectorNode).IsAssignableFrom(t)) return new Color(0.62f, 0.45f, 0.92f);
        if (typeof(SequenceNode).IsAssignableFrom(t)) return new Color(0.25f, 0.72f, 0.78f);
        if (typeof(ConditionNode).IsAssignableFrom(t)) return new Color(0.92f, 0.74f, 0.28f);
        return new Color(0.38f, 0.78f, 0.42f);
    }
}
