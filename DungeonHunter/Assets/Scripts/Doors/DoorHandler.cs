using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomType { NULL, QuietRoom, BattleRoom, BossRoom }

[ExecuteInEditMode]
public class DoorHandler : MonoBehaviour
{
	[Header("Scripts:")]

	[SerializeField]
	private DoorMaker doorMaker;

	[SerializeField]
	private EnemyHandler enemyHandler;

	public HealthSystem playerHealth;
	public PlayerAttack playerAttack;
	public PlayerInventory playerInventory;

	// Variables

	[SerializeField]
	private RoomType roomType;

	private Door[] doors;

	private bool locked = true;

	// Enumerators

	private IEnumerator checkDoors;

	[Header ("Debug:")]

	[SerializeField]
	private bool assignEnemyHandler;

	[SerializeField]
	private bool debug_checkingDoors;

	#if UNITY_EDITOR
	private void Update ()
	{
		if (assignEnemyHandler)
		{
			assignEnemyHandler = false;

			EnemyHandler[] handlers = FindObjectsOfType<EnemyHandler> ();

			if (handlers.Length == 0) Debug.LogWarning ("No Enemy Handlers found.");
			else if (handlers.Length > 1) Debug.LogWarning ("More than 1 Enemy Handler in scene.");
			else enemyHandler = handlers[0];
		}
	}
	#endif

	public void Prep ()
	{
		// Check Locks

		switch (roomType)
		{
			case RoomType.QuietRoom:
				locked = false;
				break;
			case RoomType.NULL:
				locked = false;
				Debug.LogWarning ("Room Type NOT set.");
				break;
		}

		// Make Doors

		doorMaker.PlaceDoors(SceneManager.GetActiveScene ().name);

		// Get Doors in Scene

		doors = DoorsFromContainer (transform);

		// Disable All Doors

		foreach (Door door in doors)
		{
			door.transform.GetComponent<CircleCollider2D> ().enabled = false;
		}
	}

	public void Execute ()
	{
		checkDoors = CheckDoors ();
		StartCoroutine (checkDoors);
	}

	private IEnumerator CheckDoors ()
	{
		while (locked)
		{
			if (debug_checkingDoors) print ("Checking doors!");

			yield return null;
		}

		// Enable all Doors
		print (doors.Length);
		foreach (Door door in doors)
		{
			door.transform.GetComponent<CircleCollider2D> ().enabled = true;
		}

		while (enabled)
		{
			foreach (Door door in doors)
			{
				//if (door.triggered && InputMatchDoorDir (door))
				if (door.triggered) ChangeScene (door);
			}

			yield return null;
		}
	}

	private void ChangeScene (Door door)
	{
		// Player Health

		DataPasser.DPInstance.playerCurrHp = playerHealth.currHp;
		DataPasser.DPInstance.playerFullHp = playerHealth.fullHp;

		// Record Player Current Damage & Attack Effect

		if (playerAttack.DamageChanged ())
			DataPasser.DPInstance.currDamage = playerAttack.CurrentDamage ();

		if (playerAttack.CurrentAttackEffect () != Attack_Effect.NULL)
			DataPasser.DPInstance.currAttackEffect = playerAttack.CurrentAttackEffect ();

		// Player Inventory

		if (playerInventory.HasItems ())
			DataPasser.DPInstance.inventoryItems = playerInventory.ItemsAsArray ();

		// Record Room & Door Direction

		DataPasser.DPInstance.previousRoom = SceneManager.GetActiveScene ().name;
		//DataPasser.DPInstance.previousDoorDir = door.direction;

		// Load Next Scene

		SceneManager.LoadScene (door.transform.name);
	}

	private Door[] DoorsFromContainer(Transform container)
	{
		Door[] tempDoors = new Door[container.childCount];

		for (int i = 0; i < tempDoors.Length; i++)
		{
			tempDoors[i] = container.GetChild(i).GetComponent<Door>();
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

	public void Unlock ()
	{
		locked = false;

		#if UNITY_EDITOR
		print ("Door Unlocked!");
		#endif
	}
}