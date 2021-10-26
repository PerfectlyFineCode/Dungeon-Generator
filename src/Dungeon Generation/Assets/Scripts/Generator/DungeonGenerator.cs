using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	private const int MaxRetries = 200;
	private const int MaxDepth = 1000;

	public void Generate(Vector3 startPoint, RoomCollectionData data)
	{
		if (data.StartRoom.Room == null) return;
		var startRoom = Instantiate(data.StartRoom.Room);
		if (!startRoom.TryGetComponent(out RoomInformation information)) return;
		information.Name = "Start";
		CurrentRooms     = new List<RoomInformation>(new[] { information });
		StartCoroutine(GenerateRoom(data, information, information));
	}

	private List<RoomInformation> CurrentRooms;
	private Dictionary<int, Bounds> BoundsList = new Dictionary<int, Bounds>();

	private IEnumerator GenerateRoom(RoomCollectionData data, RoomInformation startRoom,
		RoomInformation previousRoom = null, int currentDepth = 0)
	{
		if (currentDepth >= MaxDepth) yield break;
		Bounds   currentBounds = default;
		RoomData currentRoom   = null;
		var      retries       = 0;
		BoundsList.Add(currentDepth, currentBounds);
		do
		{
			if (retries >= MaxRetries) break;
			retries++;
			var roomData = data.Rooms.Random();
			var endPoint = roomData.GetRoomBounds.RandomPoint();

			var startPoint = startRoom.GetRandomPath;

			var roomSize           = roomData.GetRoomBounds.size;
			var currentRoomExtents = startRoom.Bounds.extents;
			var roomExtents        = roomData.GetRoomBounds.extents;

			var pos = startPoint + endPoint;

			currentBounds = new Bounds(pos, roomSize);
			Debug.Log("Generating new ..");
			BoundsList[currentDepth] = currentBounds;
			yield return new WaitForSeconds(0.5f);
			currentRoom = roomData;
		} while (IntersectsAny(CurrentRooms, currentBounds));

		if (currentRoom == null || IntersectsAny(CurrentRooms, currentBounds)) yield break;

		var roomObject      = Instantiate(currentRoom.Room, currentBounds.center, Quaternion.identity);
		var roomInformation = roomObject.GetComponent<RoomInformation>();
		roomInformation.Parent = previousRoom;
		roomInformation.Name   = $"{currentDepth}";
		CurrentRooms.Add(roomInformation);
		StartCoroutine(GenerateRoom(data, startRoom, roomInformation, currentDepth + 1));
		Debug.Log("Success");
	}

	private static bool IntersectsAny(IEnumerable<RoomInformation> informations, Bounds bounds)
	{
		return informations.Any(room => room.Bounds.Intersects(bounds));
	}

	private void OnDrawGizmos()
	{
		if (BoundsList != null)
			foreach (var bounds in BoundsList.Values)
				Gizmos.DrawWireCube(bounds.center, bounds.size);

		if (CurrentRooms == null) return;
		foreach (var info in CurrentRooms)
		{
			if (info.Parent == null) continue;
			Gizmos.DrawLine(info.transform.position, info.Parent.transform.position);
		}
	}
}