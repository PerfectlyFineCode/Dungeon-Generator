using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	private const int MaxRetries = 30;

	public void Generate(Vector3 startPoint, RoomCollectionData data)
	{
		if (data.StartRoom.Room == null) return;
		var startRoom = Instantiate(data.StartRoom.Room);
		if (!startRoom.TryGetComponent(out RoomInformation information)) return;
		information.Name = "Start";
		CurrentRooms     = new List<RoomInformation>(new[] { information });
		GenerateRoom(ref data, information, information);
	}

	private List<RoomInformation> CurrentRooms;

	private void GenerateRoom(ref RoomCollectionData data, RoomInformation startRoom,
		RoomInformation previousRoom = null, int currentDepth = 0, int number = 0)
	{
		Bounds   currentBounds = default;
		RoomData currentRoom   = null;
		var      retries       = 0;

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

			var pos = startPoint
			          + new Vector3(endPoint.x * roomExtents.magnitude, 0, endPoint.z * roomExtents.magnitude);

			currentBounds = new Bounds(pos, roomSize);
			Debug.Log("Generating new ..");
			currentRoom = roomData;
		} while (IntersectsAny(CurrentRooms, currentBounds));

		if (currentRoom == null || IntersectsAny(CurrentRooms, currentBounds)) return;

		var roomObject      = Instantiate(currentRoom.Room, currentBounds.center, Quaternion.identity);
		var roomInformation = roomObject.GetComponent<RoomInformation>();
		roomInformation.Parent = previousRoom;
		roomInformation.Name   = $"{number}";
		CurrentRooms.Add(roomInformation);
		GenerateRoom(ref data, startRoom, roomInformation, number + 1);
		Debug.Log("Success");
	}

	private static bool IntersectsAny(IEnumerable<RoomInformation> informations, Bounds bounds)
	{
		return informations.Any(room => room.Bounds.Intersects(bounds));
	}

	private void OnDrawGizmos()
	{
		if (CurrentRooms == null) return;
		foreach (var info in CurrentRooms)
		{
			if (info.Parent == null) continue;
			Gizmos.DrawLine(info.transform.position, info.Parent.transform.position);
		}
	}
}