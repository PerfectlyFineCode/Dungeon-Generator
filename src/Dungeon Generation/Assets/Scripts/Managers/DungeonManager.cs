using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
	public DungeonGenerator Generator;
	private DungeonGeneratorExtended Extended;
	public Vector3 StartPoint;

	[field: SerializeField] public RoomCollectionData RoomCollectionData { get; set; }

	private void Awake()
	{
		Generator = gameObject.AddComponent<DungeonGenerator>();
		// Extended = gameObject.AddComponent<DungeonGeneratorExtended>();
	}

	public void Generate()
	{
		if (!Generator.CanRefresh) return;
		Generator.Generate(StartPoint, RoomCollectionData);
	}
}