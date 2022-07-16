using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(DataVisual)), CanEditMultipleObjects]
public class StageSOEditor : Editor
{
	GUIContent prefabLabel;
	SerializedProperty prefabProp;

	GUIContent requireMPLabel;
	SerializedProperty requireMP;

	GUIContent explainLabel;
	SerializedProperty explain;

	GUIContent gradeLabel;
	SerializedProperty grade;

	GUIContent attributeLabel;
	SerializedProperty attribute;

	GUIContent editLabel;
	SerializedProperty edit;


	private void OnEnable()
	{
		prefabLabel = new GUIContent("Magicクラスを継承したプレハブ");
		gradeLabel = new GUIContent("魔法等級");
		attributeLabel = new GUIContent("魔法属性");
		requireMPLabel = new GUIContent("発動MP");
		explainLabel = new GUIContent("説明");
		editLabel = new GUIContent("カスタム編集");

		prefabProp = serializedObject.FindProperty("prefab");
		requireMP = serializedObject.FindProperty("requireMP");
		explain = serializedObject.FindProperty("explain");
		grade = serializedObject.FindProperty("grade");
		attribute = serializedObject.FindProperty("attribute");
		edit = serializedObject.FindProperty("edit");
	}
	public override void OnInspectorGUI()
	{
		// 最新データを取得
		serializedObject.Update();

		EditorGUILayout.HelpBox("モード設定", MessageType.None);
		EditorGUILayout.PropertyField(prefabProp, prefabLabel);
		EditorGUILayout.PropertyField(edit, editLabel);
		EditorGUILayout.Space(20);

		if (edit.boolValue)
        {
			EditorGUILayout.HelpBox("カスタム編集", MessageType.None);
			EditorGUILayout.PropertyField(grade, gradeLabel);
			EditorGUILayout.PropertyField(attribute, attributeLabel);
			EditorGUILayout.PropertyField(requireMP, requireMPLabel);
			EditorGUILayout.PropertyField(explain, explainLabel);
		}
        else
        {
			EditorGUILayout.HelpBox("詳細内容", MessageType.None);
			EditorGUILayout.LabelField($"魔法等級：{ContextName.GradeName(grade.intValue)}");
			EditorGUILayout.LabelField($"魔法属性：{ContextName.AtributeName(attribute.intValue)}");
			EditorGUILayout.LabelField($"発動MP：{requireMP.floatValue}");
			EditorGUILayout.LabelField($"説明：{explain.stringValue}");
        }

		serializedObject.ApplyModifiedProperties();
	}
}

//[CustomEditor(typeof(UICButton)), CanEditMultipleObjects]
//public class UICButtonEditor : Editor
//{
//	SerializedProperty triggerTag;

//	private void OnEnable()
//	{
//		triggerTag = serializedObject.FindProperty("triggerTag");
//	}
//	public override void OnInspectorGUI()
//	{
//		base.OnInspectorGUI();
//		serializedObject.Update();
//		triggerTag.stringValue = EditorGUILayout.TagField("TriggerTag", triggerTag.stringValue);
//		serializedObject.ApplyModifiedProperties();
//	}
//}
#endif