using UnityEngine;

public class DungeonGeneratorExtended
{
	public static void Generate(RoomCollectionData data)
	{
		var points = new Vector2[150];
		for (var i = 0; i < points.Length; i++)
			points[i] = RandomPointInCircle(5);
	}

	private static Vector2 RandomPointInCircle(float radius = 5)
	{
		return Random.insideUnitCircle * radius;
	}
}