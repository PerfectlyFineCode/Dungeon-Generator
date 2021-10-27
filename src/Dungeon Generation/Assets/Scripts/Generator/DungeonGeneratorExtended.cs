using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGeneratorExtended : MonoBehaviour
{
	public void Generate(RoomCollectionData data)
	{
		var points = new Vector3[150];
		for (var i = 0; i < points.Length; i++)
			points[i] = RandomPointInCircle(5);

		var rooms = new RoomData[points.Length];
		for (var i = 0; i < rooms.Length; i++)
			rooms[i] = data.Rooms.Random();

		var roomTransforms = new RoomInformation[rooms.Length];
		for (var i = 0; i < roomTransforms.Length; i++)
			roomTransforms[i] = Instantiate(
					rooms[i].Room,
					points[i],
					Quaternion.identity)
				.GetComponent<RoomInformation>();

		StartCoroutine(Spread(data, roomTransforms, points.Select(x => x.normalized).ToArray()));
	}

	private static Vector3 RandomPointInCircle(float radius = 5)
	{
		Vector3 random = Random.insideUnitSphere;
		return new Vector3(random.x, 0, random.z) * radius;
	}

	private IEnumerator Spread(RoomCollectionData data, RoomInformation[] informations, Vector3[] positions)
	{
		var values = new float[informations.Length];

		while (AnythingCollides(informations))
			for (var i = 0; i < informations.Length; i++)
			{
				Vector3 pos = informations[i].transform.position;
				float t = values[i] += 0.64f * i;
				informations[i].transform.position = SnapPosition(pos + positions[i] * t, 0.64f);
				yield return new WaitForSeconds(0.1f);
			}
	}

	private float RoomSize(Bounds bounds)
	{
		return (bounds.size.x + bounds.size.z - 1) / bounds.size.z * bounds.size.z;
	}

	private static Vector3 SnapPosition(Vector3 position, float size)
	{
		return new Vector3(
			position.x - position.x % size,
			position.y - position.y % size,
			position.z - position.z % size);
	}

	private bool AnythingCollides(RoomInformation[] informations)
	{
		return informations.Any(a => informations.Any(b => a.Bounds.Intersects(b.Bounds)));
	}
}