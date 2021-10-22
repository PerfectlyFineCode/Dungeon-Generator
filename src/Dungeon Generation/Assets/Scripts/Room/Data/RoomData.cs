using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData_", menuName = "Data/Room")]
public class RoomData : ScriptableObject
{
	[field: SerializeField] public GameObject Room { get; set; }
}