using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Room
{
	public string roomName;
	public Vector2Int pos;
}

public class RoomPlotter : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject roomPrefab;
	[SerializeField]
	private GameObject currRoomGO;

	[Header ("Variables:")]

	[SerializeField]
	private Room currRoom;
	[SerializeField]
	private List<Room> roomList = new List<Room> ();

	private void OnEnable()
	{
		// Create First Room

		if (roomList.Count <= 0)
		{
			roomList.Add (new Room { roomName = "Entrance",pos = new Vector2Int () });

			currRoom = roomList[0];

			currRoomGO = Instantiate (roomPrefab, new Vector3 (), Quaternion.identity, transform);
			currRoomGO.name = currRoom.roomName;
			currRoomGO.GetComponent<SpriteRenderer> ().color = Color.green;

			return;
		}
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			// Check for Current Room

			if (currRoom.roomName == "")
			{
				Debug.LogError ("Current Room NOT Defined");
				return;
			}

			// Check Current Room in List

			bool roomFound = false;
			
			foreach (Room room in roomList)
			{
				if (room.roomName == currRoom.roomName) roomFound = true;
			}

			if (!roomFound)
			{
				Debug.LogError ("Defined Room NOT Found in List.");
				return;
			}

			// Create Room by Direction

			if (Input.GetAxisRaw ("Vertical") > 0) // Up
			{
				CheckAndCreateRoom (currRoom.pos + new Vector2Int (0, 1));
				return;
			}

			if (Input.GetAxisRaw ("Vertical") < 0) // Down
			{
				CheckAndCreateRoom (currRoom.pos + new Vector2Int (0, -1));
				return;
			}

			if (Input.GetAxisRaw ("Horizontal") < 0) // Left
			{
				CheckAndCreateRoom (currRoom.pos + new Vector2Int (-1, 0));
				return;
			}

			if (Input.GetAxisRaw ("Horizontal") > 0) // Right
			{
				CheckAndCreateRoom (currRoom.pos + new Vector2Int (1, 0));
				return;
			}
		}
	}

	private void CheckAndCreateRoom (Vector2Int pos)
	{
		Room tempRoom = GetRoom (pos);

		if (tempRoom.roomName == null)
		{
			CreateNewRoom (pos);
			return;
		}

		currRoomGO.GetComponent<SpriteRenderer> ().color = Color.gray;
		currRoom = tempRoom;
		currRoomGO = transform.Find (currRoom.roomName).gameObject;
		currRoomGO.GetComponent<SpriteRenderer> ().color = Color.green;
	}

	private Room GetRoom (Vector2Int pos)
	{
		foreach (Room room in roomList)
		{
			if (pos == room.pos) return room;
		}

		return default;
	}

	private void CreateNewRoom (Vector2Int newPos)
	{
		string newRoomName = Random.Range (0, 1000000).ToString ();

		Room newRoom = new Room { 
			roomName = newRoomName, 
			pos = newPos 
		};

		roomList.Add (newRoom);

		currRoomGO.GetComponent<SpriteRenderer> ().color = Color.gray;
		currRoomGO = Instantiate (roomPrefab, new Vector3 (newPos.x, newPos.y), Quaternion.identity, transform);
		currRoomGO.name = newRoomName;
		currRoomGO.GetComponent<SpriteRenderer> ().color = Color.green;

		currRoom = newRoom;
	}
}
