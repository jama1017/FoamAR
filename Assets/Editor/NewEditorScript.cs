using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FoamRadialMenuParent))]
public class FoamRadialMenuParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FoamRadialMenuParent frmp = (FoamRadialMenuParent)target;
        frmp.IsToolMenu = EditorGUILayout.Toggle("Is Tool Menu", frmp.IsToolMenu);

        if (frmp.IsToolMenu)
        {
            frmp.m_defaultOption = EditorGUILayout.IntField("Default Option Int", frmp.m_defaultOption);
        }
    }
}
