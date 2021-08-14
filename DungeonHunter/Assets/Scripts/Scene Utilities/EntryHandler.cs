using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHandler : MonoBehaviour
{
	public HealthSystem playerHealth;
	public PlayerInventory playerInventory;
	public DoorHandler doorHandler;
	public Transform player;

	private void Start ()
	{
		// Prep ////////////////////////////////////////////////////////////////

		// Data Passer

		if (DataPasser.DPInstance == null)
		{
			GameObject dataPasser = new GameObject { name = "Data Passer" };
			dataPasser.AddComponent<DataPasser> ();
		}
		else
		{
			// Player Health

			playerHealth.hp = DataPasser.DPInstance.playerHealth;
		}

		// Player Inventory

		if (DataPasser.DPInstance.inventoryItems != null)
		playerInventory.AddItems (DataPasser.DPInstance.inventoryItems);

		// Door Handler

		doorHandler.Prep ();

		// Position Player at Door

		if (DataPasser.DPInstance.previousRoom != "")
		{
			Vector3 newPos = doorHandler.DoorPosition (DataPasser.DPInstance.previousRoom);

			switch (DataPasser.DPInstance.previousDoorDir)
			{
				case DoorDirection.Up:
					newPos += new Vector3 (0, 1);
					break;
				case DoorDirection.Down:
					newPos += new Vector3 (0, -1);
					break;
				case DoorDirection.Right:
					newPos += new Vector3 (1, 0);
					break;
				case DoorDirection.Left:
					newPos += new Vector3 (-1, 0);
					break;
			}

			player.position = newPos;
		}

		// Execute /////////////////////////////////////////////////////////////

		// Door Handler

		doorHandler.Execute ();
	}
}
