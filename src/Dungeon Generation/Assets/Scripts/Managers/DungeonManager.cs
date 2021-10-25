using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
	private DungeonGenerator Generator;

	public Vector3 StartPoint;

	[field: SerializeField] public RoomCollectionData RoomCollectionData { get; set; }

	private void Awake()
	{
		Generator = gameObject.AddComponent<DungeonGenerator>();
	}

	// Start is called before the first frame update
	private void Start()
	{
		Generator.Generate(StartPoint, RoomCollectionData);
	}
}