using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BehaviourTreeSO))]
public class BehaviourTreeSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var so = (BehaviourTreeSO)target;

        EditorGUILayout.Space(6);
        if (GUILayout.Button("Open Behaviour Tree Editor", GUILayout.Height(36)))
            BehaviourTreeEditorWindow.Open(so);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Cây hiện tại", EditorStyles.boldLabel);
        if (so.Root == null)
            EditorGUILayout.HelpBox("Root trống. Mở editor để chọn Selector / Sequence làm gốc.", MessageType.Info);
        else
            DrawPreview(so.Root, 0);
    }

    static void DrawPreview(BTNode node, int depth)
    {
        if (node == null) return;

        string prefix = depth == 0 ? "Root" : BTNodeTypes.Category(node.GetType());
        EditorGUILayout.LabelField($"{new string('·', depth * 2)} {node.GetDisplayName()}  [{prefix}]");

        if (node is CompositeNode composite && composite.children != null)
        {
            foreach (var child in composite.children)
                DrawPreview(child, depth + 1);
        }
    }
}
