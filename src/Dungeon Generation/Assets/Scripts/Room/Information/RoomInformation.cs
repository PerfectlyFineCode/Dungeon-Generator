using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class RoomInformation : MonoBehaviour
{
	[field: SerializeField]
	public string Name { get; set; }

	[field: SerializeField]
	public List<PathBounds> PathBounds { get; set; }

	private void OnDrawGizmos()
	{
		var bounds = new Bounds(transform.position, Vector3.zero);
		foreach (MeshRenderer child in GetComponentsInChildren<MeshRenderer>()) bounds.Encapsulate(child.bounds);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(bounds.center, bounds.size);
		var style = new GUIStyle
		{
			fontSize = 19,
			normal =
			{
				textColor = Color.red
			}
		};
		Handles.Label(bounds.center + new Vector3(0, bounds.size.y, 0), $"{Name}", style);
	}
}

[Serializable]
public class PathBounds
{
	[HideInInspector] public BoxBoundsHandle BoundsHandle = new BoxBoundsHandle();

	[field: SerializeField]
	public Bounds Bounds { get; set; }
}