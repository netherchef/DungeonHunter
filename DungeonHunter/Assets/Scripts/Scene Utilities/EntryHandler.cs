using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHandler : MonoBehaviour
{
	public HealthSystem playerHealth;
	public HealthBar healthBar;
	public PlayerInventory playerInventory;
	public DoorHandler doorHandler;
	public Transform player;

	private void Start ()
	{
		// Prep ////////////////////////////////////////////////////////////////

		// Data Passer

		// !!! TEMPORARY !!!
		// If there is NO Data Passer, create one.

		if (DataPasser.DPInstance == null)
		{
			GameObject dataPasser = new GameObject { name = "Data Passer" };
			dataPasser.AddComponent<DataPasser> ();
		}

		// Player Health

		if (DataPasser.DPInstance.playerCurrHp == 0) DataPasser.DPInstance.playerCurrHp = playerHealth.currHp;
		else playerHealth.currHp = DataPasser.DPInstance.playerCurrHp;

		// Health Bar

		healthBar.Prep (playerHealth);

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
