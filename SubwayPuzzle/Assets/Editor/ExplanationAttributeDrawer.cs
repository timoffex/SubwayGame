using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ExplanationAttribute))]
public class ExplanationAttributeDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        EditorGUI.LabelField(position, InfoText, LabelStyle);
    }

    public override float GetHeight() =>
        LabelStyle.CalcHeight(
            new GUIContent(InfoText),
            EditorGUIUtility.labelWidth);

    private string InfoText =>
        (attribute as ExplanationAttribute).text;

    private GUIStyle LabelStyle =>
        new GUIStyle(GUI.skin.label)
        {
            wordWrap = true
        };
}
