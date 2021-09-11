using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { NULL, SkeletonWarrior, SkeletonPriest, Bat }

[System.Serializable]
public struct Enemy
{
	// Enemy Info

	public GameObject enemyObject;
	public EnemyInfo enemyInfo;

	// Health

	public HealthSystem healthSystem;

	// Variables

	public bool attack;
	public bool damageTarget;

	// Enumerators

	public IEnumerator attackCoroutine;
}

[ExecuteInEditMode]
public class EnemyHandler : MonoBehaviour
{
	[Header ("Components:")]

	public Transform target;
	public Transform enemyContainer;

	public GameObject skeletonWarriorPrefab;
	public GameObject skeletonPriestPrefab;
	public GameObject batPrefab;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem targetHealthSystem;
	public LootHandler lootHandler;

	public SceneBounds sceneBounds;

	[SerializeField]
	private DoorHandler doorHandler;

	[Header ("Variables:")]

	[SerializeField]
	private EnemyType[] spawnOnEntry;

	private List<HealthSystem> enemyHealths = new List<HealthSystem> ();

	// Enumerators

	private IEnumerator CheckEnemies { get { return DoCheckEnemies (); } }

	[Header ("Debug:")]

	[SerializeField]
	private bool assignDoorHandler;

	#if UNITY_EDITOR
	private void Update ()
	{
		if (assignDoorHandler)
		{
			assignDoorHandler = false;

			DoorHandler[] handlers = FindObjectsOfType<DoorHandler> ();

			if (handlers.Length == 0) Debug.LogWarning ("No Door Handlers found.");
			else if (handlers.Length > 1) Debug.LogWarning ("More than 1 Door Handler in scene.");
			else doorHandler = handlers[0];
		}
	}
	#endif

	public void Prep ()
	{
		SpawnAtRandomLocations ();

		StartCoroutine (CheckEnemies);
	}

	public void SpawnAtRandomLocations ()
	{
		foreach (EnemyType enemyType in spawnOnEntry)
		{
			switch (enemyType)
			{
				case EnemyType.SkeletonWarrior:
					Spawn_SkeletonWarrior ();
					break;
				case EnemyType.SkeletonPriest:
					Spawn_SkeletonPriest ();
					break;
				case EnemyType.Bat:
					Spawn_Bat ();
					break;
			}
		}
	}

	private IEnumerator DoCheckEnemies ()
	{
		while (enemyHealths.Count > 0)
		{
			//foreach (HealthSystem enemyHealth in enemyHealths)
			//{
			//	if (enemyHealth.Dead ()) enemyHealths.Remove (enemyHealth);
			//}

			for (int i = 0; i < enemyHealths.Count; i++)
			{
				if (enemyHealths[i].Dead ()) enemyHealths.Remove (enemyHealths[i]);
			}

			yield return null;
		}

		doorHandler.Unlock ();
	}

	public void Spawn_SkeletonWarrior (bool summon = false)
	{
		GameObject newEnemy = Instantiate (skeletonWarriorPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		SkeletonWarriorFunctions skelFuncs = newEnemy.GetComponent<SkeletonWarriorFunctions> ();

		enemyHealths.Add (skelFuncs.HealthSystem ());

		skelFuncs.lootHandler = lootHandler;

		skelFuncs.SetTarget (target);

		skelFuncs.Set_TargetHealthSystem (targetHealthSystem);

		if (summon) skelFuncs.Set_SummonStart ();

		skelFuncs.Execute (summon);
	}

	private void Spawn_SkeletonPriest ()
	{
		GameObject newEnemy = Instantiate (skeletonPriestPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		SkeletonPriestFunctions priestFuncs = newEnemy.GetComponent<SkeletonPriestFunctions> ();

		enemyHealths.Add (priestFuncs.HealthSystem ());

		priestFuncs.Set_EnemyHandler (this);

		priestFuncs.Set_LootHandler (lootHandler);

		priestFuncs.Set_TargetHealthSystem (targetHealthSystem);

		priestFuncs.Execute ();
	}

	private void Spawn_Bat ()
	{
		GameObject newEnemy = Instantiate (batPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		Bat bat = newEnemy.GetComponent<Bat> ();

		enemyHealths.Add (bat.HealthSystem ());

		bat.SetTarget (target);

		bat.Set_TargetHealth (targetHealthSystem);

		bat.Execute ();
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision)
		{
			if (spawnOnEntry[0] == EnemyType.Bat)
			{

			}
		}
	}
}