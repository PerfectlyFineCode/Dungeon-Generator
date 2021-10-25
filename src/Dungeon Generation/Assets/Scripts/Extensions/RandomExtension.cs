using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class RandomExtension
{
	private static readonly Random random = new Random();

	public static T Random<T>(this IEnumerable<T> list, Func<T, bool> condition = null)
	{
		var arr        = condition == null ? list : list.Where(condition);
		var enumerable = arr as T[] ?? arr.ToArray();

		if (!enumerable.Any()) return default;
		var randomValue = random.Next(0, enumerable.Length);

		return enumerable[randomValue];
	}

	public static Vector3 RandomPoint(this Bounds bounds)
	{
		return new Vector3(
			UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
			UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
			UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
	}
}