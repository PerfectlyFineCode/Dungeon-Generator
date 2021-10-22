using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomBounds : MonoBehaviour
{
	// Start is called before the first frame update
	private void Start()
	{
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private void OnDrawGizmos()
	{
		var bounds = new Bounds(transform.position, Vector3.zero);
		foreach (var child in GetComponentsInChildren<MeshRenderer>()) bounds.Encapsulate(child.bounds);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(bounds.center, bounds.size);
	}
}