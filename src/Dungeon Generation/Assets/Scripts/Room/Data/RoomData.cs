using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData_", menuName = "Data/Room")]
public class RoomData : ScriptableObject
{
	[field: SerializeField] public GameObject Room { get; set; }

	public Bounds GetRoomBounds => Room.GetComponentInChildren<RoomInformation>().Bounds;
}

#if UNITY_EDITOR

[CustomEditor(typeof(RoomData))]
public class RoomDataEditor : Editor
{
	private GameObject gameObject;
	private Editor gameObjectEditor;
	private bool previewVisible;

	private void OnEnable()
	{
		gameObject = (target as RoomData)?.Room;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		previewVisible = EditorGUILayout.BeginFoldoutHeaderGroup(previewVisible, "Preview");
		if (previewVisible)
		{
			var bgColor = new GUIStyle();

			if (gameObject == null) return;
			if (gameObjectEditor == null)
				gameObjectEditor = CreateEditor(gameObject);

			gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
		}

		EditorGUILayout.EndFoldoutHeaderGroup();
	}
}

#endif