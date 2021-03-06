using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class DungeonGenerator : MonoBehaviour
{
	private const int MaxRetries = 200;
	private const int MaxDepth = 1000;
	private Dictionary<int, Bounds> BoundsList = new Dictionary<int, Bounds>();
	private readonly System.Random random = new System.Random();
	public bool CanRefresh => CurrentWorkers == 0;
	private bool previousCanRefresh;
	public UnityEvent<bool> RefreshStatechanged = new UnityEvent<bool>();

	public List<RoomInformation> CurrentRooms;

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
		Refresh();
		BoundsList = new Dictionary<int, Bounds>();

		if (!startRoom.TryGetComponent(out RoomInformation information)) return;
		information.Name = "Start";
		CurrentRooms     = new List<RoomInformation>(new[] { information });
		StartCoroutine(GenerateRoom(data, information, information));
	}

	private void Refresh()
	{
		if (CurrentRooms != null && CurrentRooms.Count > 0)
			foreach (RoomInformation room in CurrentRooms.Where(x => x != null))
				Destroy(room.gameObject);
	}

	private int CurrentWorkers;

	private void CheckWorkerAvailable()
	{
		bool state = CanRefresh;
		switch (state)
		{
			case true when !previousCanRefresh:
				previousCanRefresh = true;
				RefreshStatechanged?.Invoke(true);
				break;
			case false when previousCanRefresh:
				previousCanRefresh = false;
				RefreshStatechanged?.Invoke(false);
				break;
		}
	}

	private IEnumerator GenerateRoom(RoomCollectionData data, RoomInformation startRoom,
		RoomInformation previousRoom = null, int currentDepth = 0)
	{
		Interlocked.Increment(ref CurrentWorkers);
		CheckWorkerAvailable();
		if (currentDepth > MaxDepth)
		{
			Interlocked.Decrement(ref CurrentWorkers);
			CheckWorkerAvailable();
			Debug.Log(CurrentWorkers);
			yield break;
		}

		RoomInformation start = previousRoom == null ? startRoom : previousRoom;
		Bounds currentBounds = default;
		RoomData currentRoom = null;
		var retries = 0;
		BoundsList.Add(currentDepth, currentBounds);
		do
		{
			if (retries > MaxRetries)
			{
				Interlocked.Decrement(ref CurrentWorkers);
				Debug.Log(CurrentWorkers);
				CheckWorkerAvailable();
				yield break;
			}

			retries++;
			RoomData roomData = data.Rooms.Random();
			Vector3 endPoint = roomData.GetRoomBounds.center;

			Vector3 startPoint = start.GetRandomPath;


			Vector3 roomSize = roomData.GetRoomBounds.size;
			Vector3 currentRoomExtents = startRoom.Bounds.extents;
			Vector3 roomExtents = roomData.GetRoomBounds.extents;

			var random = new Vector2(
				this.random.Next(-1, 1),
				this.random.Next(-1, 1));

			var pos = new Vector3(
				startPoint.x + endPoint.x,
				startRoom.Bounds.center.y,
				startPoint.z + endPoint.z);


			Vector3 rel = pos - start.Bounds.center;

			currentBounds = new Bounds(pos + new Vector3(rel.x, 0, rel.z), roomSize);

			Debug.Log("Generating new ..");
			BoundsList[currentDepth] = currentBounds;
			yield return new WaitForSeconds(0.01f);
			currentRoom = roomData;
		} while (IntersectsAny(CurrentRooms, currentBounds));

		if (currentRoom == null || IntersectsAny(CurrentRooms, currentBounds)) yield break;

		GameObject roomObject = Instantiate(currentRoom.Room, currentBounds.center - currentRoom.GetRoomBounds.center,
			Quaternion.identity);
		var roomInformation = roomObject.GetComponent<RoomInformation>();
		roomInformation.Parent = previousRoom;
		roomInformation.Name   = $"{currentDepth}";
		CurrentRooms.Add(roomInformation);
		BoundsList.Remove(currentDepth);
		StartCoroutine(GenerateRoom(data, startRoom, roomInformation, currentDepth + 1));
		Debug.Log("Success");
		Interlocked.Decrement(ref CurrentWorkers);
		CheckWorkerAvailable();
		Debug.Log(CurrentWorkers);
	}

	private static bool IntersectsAny(IEnumerable<RoomInformation> informations,
		Bounds bounds)
	{
		return informations.Any(room => BetterIntersects(room.Bounds, bounds));
	}

	private static bool BetterIntersects(Bounds a, Bounds b)
	{
		Vector3 min = a.min;
		Vector3 max = a.max;

		return min.x < b.max.x && max.x > b.min.x
		                       && min.y < b.max.y && max.y > b.min.y
		                       && min.z < b.max.z && max.z > b.min.z;
	}
}