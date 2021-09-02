using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour
{
	[Header ("Components:")]

	public Transform doorContainer;

	[Header ("Scripts:")]

	public HealthSystem playerHealth;
	public PlayerAttack playerAttack;
	public PlayerInventory playerInventory;

	// Variables

	private Door[] doors;

	// Enumerators

	private IEnumerator checkDoors;

	// !!! TEMPORARY !!!
	//private void Start ()
	//{
	//	Prep ();
	//	Execute ();
	//}

	public void Prep ()
	{
		doors = DoorsFromContainer (doorContainer);
	}

	public void Execute ()
	{
		checkDoors = CheckDoors ();
		StartCoroutine (checkDoors);
	}

	private IEnumerator CheckDoors ()
	{
		while (enabled)
		{
			foreach (Door door in doors)
			{
				if (door.triggered && InputMatchDoorDir (door)) ChangeScene (door);
			}

			yield return null;
		}
	}

	private bool InputMatchDoorDir (Door door)
	{
		if (Input.GetAxisRaw ("Horizontal") > 0 && door.direction == DoorDirection.Right) return true;
		if (Input.GetAxisRaw ("Horizontal") < 0 && door.direction == DoorDirection.Left) return true;
		if (Input.GetAxisRaw ("Vertical") > 0 && door.direction == DoorDirection.Up) return true;
		if (Input.GetAxisRaw ("Vertical") < 0 && door.direction == DoorDirection.Down) return true;

		return false;
	}

	private void ChangeScene (Door door)
	{
		// Player Health

		DataPasser.DPInstance.playerCurrHp = playerHealth.currHp;
		DataPasser.DPInstance.playerFullHp = playerHealth.fullHp;

		// Record Player Current Damage

		if (playerAttack.DamageChanged ())
			DataPasser.DPInstance.currDamage = playerAttack.CurrentDamage ();

		// Player Inventory

		if (playerInventory.HasItems ())
			DataPasser.DPInstance.inventoryItems = playerInventory.ItemsAsArray ();

		// Record Room & Door Direction

		DataPasser.DPInstance.previousRoom = SceneManager.GetActiveScene ().name;
		DataPasser.DPInstance.previousDoorDir = door.direction;

		// Load Next Scene

		SceneManager.LoadScene (door.transform.name);
	}

	private Door[] DoorsFromContainer (Transform container)
	{
		Door[] tempDoors = new Door[container.childCount];

		for (int i = 0; i < tempDoors.Length; i++)
		{
			tempDoors[i] = container.GetChild (i).GetComponent<Door> ();
		}

		return tempDoors;
	}

	public Vector3 DoorPosition (string doorName)
	{
		foreach (Door door in doors)
		{
			if (door.gameObject.name == doorName) return door.transform.position;
		}

		return new Vector3 (0, 0);
	}
}