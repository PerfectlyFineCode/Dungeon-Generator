using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(RoomInformation))]
public class PathBoundsEditor : Editor
{
	private RoomInformation _information;

	private void OnEnable()
	{
		_information = target as RoomInformation;
	}

	private void OnSceneGUI()
	{
		if (_information.PathBounds == null) return;
		var paths = _information.PathBounds;
		EditorGUI.BeginChangeCheck();
		foreach (PathBounds t in paths)
		{
			Bounds path = t.Bounds;
			t.BoundsHandle.center = path.center;
			t.BoundsHandle.size = path.size;
			t.BoundsHandle.DrawHandle();
			t.BoundsHandle.center = Handles.PositionHandle(path.center, Quaternion.identity);
		}

		if (!EditorGUI.EndChangeCheck()) return;
		foreach (PathBounds t in paths)
		{
			Undo.RecordObject(target, "Moved paths");
			BoxBoundsHandle handle = t.BoundsHandle;
			t.Bounds = new Bounds(handle.center, handle.size);
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}