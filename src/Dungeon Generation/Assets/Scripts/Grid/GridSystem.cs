using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
	public static Node[,] Nodes { get; private set; }

	public static void Initialize(int x, int y)
	{
		Nodes = new Node[x, y];
	}

	public static void Resize(int x, int y)
	{
	}

	public static void SetSize(int a, int b)
	{
	}
}

public class Node
{
	public bool Blocking { get; private set; }

	public void SetBlocking(bool blocking)
	{
		Blocking = blocking;
	}
}