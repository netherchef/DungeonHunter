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

public class EnemyHandler : MonoBehaviour
{
	[Header ("Components:")]

	public Transform target;
	public Transform enemyContainer;

	public GameObject skeletonWarriorPrefab;
	public GameObject skeletonPriestPrefab;
	public GameObject batPrefab;

	[Header ("Scripts:")]

	public HealthSystem targetHealthSystem;
	public LootHandler lootHandler;

	public SceneBounds sceneBounds;

	[Header ("Variables:")]

	[SerializeField]
	private EnemyType[] spawnOnEntry;

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep () { SpawnAtRandomLocations (); }

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

	public void Spawn_SkeletonWarrior ()
	{
		GameObject newEnemy = Instantiate (skeletonWarriorPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		SkeletonWarriorFunctions skelFuncs = newEnemy.GetComponent<SkeletonWarriorFunctions> ();

		skelFuncs.lootHandler = lootHandler;

		skelFuncs.SetTarget (target);

		skelFuncs.Set_TargetHealthSystem (targetHealthSystem);

		skelFuncs.Execute ();
	}

	private void Spawn_SkeletonPriest ()
	{
		GameObject newEnemy = Instantiate (skeletonPriestPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		SkeletonPriestFunctions priestFuncs = newEnemy.GetComponent<SkeletonPriestFunctions> ();

		priestFuncs.Set_EnemyHandler (this);

		priestFuncs.Set_LootHandler (lootHandler);

		priestFuncs.Execute ();
	}

	private void Spawn_Bat ()
	{
		GameObject newEnemy = Instantiate (batPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		Bat bat = newEnemy.GetComponent<Bat> ();

		bat.SetTarget (target);

		bat.Set_TargetHealth (targetHealthSystem);

		bat.Execute ();
	}
}