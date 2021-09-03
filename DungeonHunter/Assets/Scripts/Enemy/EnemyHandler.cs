using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { NULL, SkeletonWarrior, SkeletonPriest }

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

	public GameObject skeletonPrefab;

	[Header ("Scripts:")]

	public HealthSystem targetHealthSystem;
	public LootHandler lootHandler;

	public SceneBounds sceneBounds;

	[Header ("Variables:")]

	[SerializeField]
	private EnemyType[] spawnOnEntry;

	//private Enemy[] enemies = new Enemy[0];

	// Enumerators

	//private IEnumerator checkEnemies;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		//Execute ();
	}

	public void Prep ()
	{
		SpawnAtRandomLocations (skeletonPrefab, spawnOnEntry.Length);
	}

	//public void Execute ()
	//{
	//	StartCoroutine (CheckEnemies ());
	//}

	//private IEnumerator CheckEnemies ()
	//{
	//	while (enabled)
	//	{
	//		for (int c = 0; c < enemies.Length; c++)
	//		{
	//			if (!enemies[c].healthSystem.Dead ())
	//			{
	//				// Chase

	//				float gapToTarg = Vector3.Magnitude (enemies[c].enemyObject.transform.position - target.position);

	//				if (!enemies[c].attack)
	//				{
	//					if (gapToTarg > enemies[c].enemyInfo.attackRange)
	//					{
	//						Vector3 moveValue = Vector3.Normalize (target.position - enemies[c].enemyObject.transform.position);
	//						moveValue.z = 0;

	//						enemies[c].enemyObject.transform.position += moveValue * enemies[c].enemyInfo.moveSpeed * Time.deltaTime;
	//					}
	//				}

	//				// Check Attack

	//				if (gapToTarg < enemies[c].enemyInfo.attackRange)
	//				{
	//					if (!enemies[c].attack) enemies[c].attack = true;
	//				}

	//				 //Remove Health

	//				if (enemies[c].damageTarget)
	//				{
	//					enemies[c].damageTarget = false;

	//					targetHealthSystem.Damage ();
	//				}
	//			}
	//			else
	//			{
	//				// Death

	//				if (enemies[c].enemyObject.activeSelf)
	//				{
	//					enemies[c].enemyObject.SetActive (false);

	//					// Drop Loot

	//					ItemType[] items = new ItemType[1];
	//					items[0] = ItemType.Gold;

	//					lootHandler.DropLoot (items, enemies[c].enemyObject.transform.position);
	//				}
	//			}
	//		}

	//		yield return null;
	//	}
	//}

	//private IEnumerator AttackCoroutine (int index)
	//{
	//	while (enemies[index].healthSystem.currHp > 0)
	//	{
	//		if (enemies[index].attack)
	//		{
	//			EnemyInfo info = enemies[index].enemyInfo;

	//			// Wind Up

	//			for (float windupTimer = info.windUp; windupTimer > 0; windupTimer -= Time.deltaTime) yield return null;

	//			// Attack Animation

	//			info.enemyAnimatorFunctions.AttackStart ();

	//			// Attack

	//			bool foundTarget = false;

	//			while (!info.enemyAnimatorFunctions.Is_AttackDone ())
	//			{
	//				if (!foundTarget)
	//				{
	//					if (Vector3.Magnitude (target.position - transform.GetChild (index).position) < info.attackRange)
	//					{
	//						foundTarget = true;
	//						enemies[index].damageTarget = true;
	//					}
	//				}

	//				yield return null;
	//			}

	//			// Cooldown

	//			for (float cdTimer = info.coolDown; cdTimer > 0; cdTimer -= Time.deltaTime) yield return null;

	//			// Reset

	//			enemies[index].attack = false;
	//		}

	//		yield return null;
	//	}
	//}

	public void SpawnAtRandomLocations (GameObject enemyPrefab, int count = 1)
	{
		while (count > 0)
		{
			// Spawn Enemy

			GameObject newEnemy = Instantiate (skeletonPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

			SkeletonWarriorFunctions skelFuncs = newEnemy.GetComponent<SkeletonWarriorFunctions> ();

			skelFuncs.lootHandler = lootHandler;

			skelFuncs.SetTarget (target);

			skelFuncs.Set_TargetHealthSystem (targetHealthSystem);

			skelFuncs.Execute ();

			//Enemy[] tempEnemies = new Enemy[enemies.Length + 1];

			//enemies.CopyTo (tempEnemies, 0);

			//tempEnemies[tempEnemies.Length - 1] = new Enemy
			//{
			//	enemyObject = newEnemy,
			//	enemyInfo = newEnemy.GetComponent<EnemyInfo> (),
			//	healthSystem = newEnemy.GetComponent<HealthSystem> ()
			//};

			//enemies = tempEnemies;

			//enemies[enemies.Length - 1].attackCoroutine = AttackCoroutine (enemies.Length - 1);

			//StartCoroutine (enemies[enemies.Length - 1].attackCoroutine);

			count--;
		}
	}
}