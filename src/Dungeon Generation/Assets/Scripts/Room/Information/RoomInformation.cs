using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif
using UnityEngine;

public class RoomInformation : MonoBehaviour
{
	[field: SerializeField] public RoomInformation Parent { get; set; }

	public Bounds Bounds
	{
		get
		{
			var bounds = new Bounds(transform.position, Vector3.zero);
			foreach (MeshRenderer child in GetComponentsInChildren<MeshRenderer>()) bounds.Encapsulate(child.bounds);
			return bounds;
		}
	}

	[field: SerializeField] public string Name { get; set; }

	[field: SerializeField] public List<PathBounds> PathBounds { get; set; }

	public Vector3 GetRandomPath
	{
		get
		{
			Vector3 path = PathBounds.Select(x => x.Bounds.center).Random();
			return new Vector3(path.x, 0, path.z);
		}
	}

	#if UNITY_EDITOR
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
	#endif
}

[Serializable]
public class PathBounds
{
	#if UNITY_EDITOR
	[HideInInspector] public BoxBoundsHandle BoundsHandle = new BoxBoundsHandle();
	#endif

	[field: SerializeField] public Bounds Bounds { get; set; }
}