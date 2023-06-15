using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FlagSystem))]
public class FlagSystemEditor : Editor
{
    private SerializedProperty stageFlagsProperty;
    private SerializedProperty testsProperty;

    private void OnEnable()
    {
        stageFlagsProperty = serializedObject.FindProperty("stageFlags");
        testsProperty = serializedObject.FindProperty("tests");
    }

    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();

    //    EditorGUI.LabelField(new Rect(new Vector2(30, 30), new Vector2(30, 30)), "¿ì¿À¿Ê");
    //}
}
