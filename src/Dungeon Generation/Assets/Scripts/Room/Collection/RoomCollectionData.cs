using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomCollectionData_", menuName = "Data/Collection/RoomData")]
public class RoomCollectionData : ScriptableObject
{
	[field: SerializeField] public RoomData StartRoom { get; set; }
	[field: SerializeField] public List<RoomData> Rooms { get; set; }
}