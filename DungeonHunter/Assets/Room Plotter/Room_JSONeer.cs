using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomList_JSON
{
	public List<Room> roomList;
}

public class Room_JSONeer : MonoBehaviour
{
	[Header ("Scripts:")]

	[SerializeField]
	private RoomPlotter roomPlotter;

	[Header ("Variables:")]

	[SerializeField]
	private bool write, read;

	private RoomList_JSON roomListJSON = new RoomList_JSON ();

	private void Update()
	{
		if (write)
		{
			write = !write;

			WriteToJSON ();
		}
	}

	public void WriteToJSON ()
	{
		roomListJSON.roomList = roomPlotter.RoomList ();

		string rlj = JsonUtility.ToJson (roomListJSON, true);
		System.IO.File.WriteAllText (
			Application.persistentDataPath + "/RoomData.json", rlj);
	}
}