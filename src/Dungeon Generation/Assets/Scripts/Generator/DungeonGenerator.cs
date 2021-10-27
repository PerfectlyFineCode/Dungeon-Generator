using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
	private const int MaxRetries = 200;
	private const int MaxDepth = 1000;
	private readonly Dictionary<int, Bounds> BoundsList = new Dictionary<int, Bounds>();

	private List<RoomInformation> CurrentRooms;

	private void OnDrawGizmos()
	{
		if (BoundsList != null)
			foreach (Bounds bounds in BoundsList.Values)
				Gizmos.DrawWireCube(bounds.center, bounds.size);

		if (CurrentRooms == null) return;
		foreach (RoomInformation info in CurrentRooms)
		{
			if (info.Parent == null) continue;
			Gizmos.DrawLine(info.transform.position, info.Parent.transform.position);
		}
	}

	public void Generate(Vector3 startPoint, RoomCollectionData data)
	{
		if (data.StartRoom.Room == null) return;
		GameObject startRoom = Instantiate(data.StartRoom.Room);
		if (!startRoom.TryGetComponent(out RoomInformation information)) return;
		information.Name = "Start";
		CurrentRooms     = new List<RoomInformation>(new[] { information });
		StartCoroutine(GenerateRoom(data, information, information));
	}

	private IEnumerator GenerateRoom(RoomCollectionData data, RoomInformation startRoom,
		RoomInformation previousRoom = null, int currentDepth = 0)
	{
		if (currentDepth > MaxDepth) yield break;
		RoomInformation start = previousRoom == null ? startRoom : previousRoom;
		Bounds currentBounds = default;
		RoomData currentRoom = null;
		var retries = 0;
		BoundsList.Add(currentDepth, currentBounds);
		do
		{
			if (retries > MaxRetries) break;
			retries++;
			RoomData roomData = data.Rooms.Random();
			Vector3 endPoint = roomData.GetRoomBounds.center;

			Vector3 startPoint = start.GetRandomPath;


			Vector3 roomSize = roomData.GetRoomBounds.size;
			Vector3 currentRoomExtents = startRoom.Bounds.extents;
			Vector3 roomExtents = roomData.GetRoomBounds.extents;

			var pos = new Vector3(
				startPoint.x + endPoint.x,
				startRoom.Bounds.center.y,
				startPoint.z + endPoint.z);


			currentBounds = new Bounds(pos + new Vector3(roomSize.x, 0, roomSize.z), roomSize);
			Debug.Log("Generating new ..");
			BoundsList[currentDepth] = currentBounds;
			yield return new WaitForSeconds(0.05f);
			currentRoom = roomData;
		} while (IntersectsAny(CurrentRooms, currentBounds));

		if (currentRoom == null || IntersectsAny(CurrentRooms, currentBounds)) yield break;

		GameObject roomObject = Instantiate(currentRoom.Room, currentBounds.center - currentRoom.GetRoomBounds.center,
			Quaternion.identity);
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
}