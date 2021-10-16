﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.UI;
#endif

public enum EnemyType { NULL, SkeletonWarrior, SkeletonPriest, Bat, AncientGuard, Slime }

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

//[ExecuteInEditMode]
public class EnemyHandler : MonoBehaviour
{
	[Header ("Enemies:")]

	[SerializeField]
	private EnemyType[] spawnOnEntry;

	[Header ("Components:")]

	public Transform target;
	[SerializeField]
	private CircleCollider2D targetCollider;

	public Transform enemyContainer;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem targetHealthSystem;
	public LootHandler lootHandler;

	public SceneBounds sceneBounds;

	[SerializeField]
	private DoorHandler doorHandler;

	[Header ("Prefabs:")]

	[SerializeField]
	private GameObject skeletonWarriorPrefab;
	[SerializeField]
	private GameObject skeletonPriestPrefab;
	[SerializeField]
	private GameObject batPrefab;
	[SerializeField]
	private GameObject ancientGuardPrefab;
	[SerializeField]
	private GameObject slimePrefab;

	[Header ("Variables:")]

	private List<HealthSystem> enemyHealths = new List<HealthSystem> ();

	// Enumerators

	private IEnumerator CheckEnemies { get { return DoCheckEnemies (); } }

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
				case EnemyType.AncientGuard:
					Spawn_AncientGuard ();
					break;
				case EnemyType.Slime:
					Spawn_Slime ();
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

		skelFuncs.Set_LootHandler (lootHandler);

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

	private void Spawn_AncientGuard ()
	{
		GameObject newEnemy = Instantiate (ancientGuardPrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		AncientGuardFunctions ag = newEnemy.GetComponent<AncientGuardFunctions> ();

		enemyHealths.Add (ag.HealthSystem ());

		ag.Set_TargetTransform (target);

		ag.Set_TargetHealthSystem (targetHealthSystem);

		ag.Set_TargetCollider (targetCollider);

		ag.Set_LootHandler (lootHandler);

		ag.Execute ();
	}

	private void Spawn_Slime ()
	{
		GameObject newEnemy = Instantiate (slimePrefab, sceneBounds.RandomPointInBounds (), Quaternion.identity, enemyContainer);

		SlimeFunctions sf = newEnemy.GetComponent<SlimeFunctions> ();

		enemyHealths.Add (sf.HealthSystem ());

		sf.Set_TargetTransform (target);

		sf.Set_TargetHealthSystem (targetHealthSystem);

		sf.Set_TargetCollider (targetCollider);

		sf.Set_LootHandler (lootHandler);

		sf.Set_SceneBounds (sceneBounds);

		sf.Execute ();
	}

#if UNITY_EDITOR

	//[SerializeField]
	//private bool showHealth;

	//[SerializeField]
	//private bool assignDoorHandler;

	//private void Start ()
	//{
	//	if (showHealth)
	//	{
	//		print ("Do");
	//		showHealth = false;

	//		GameObject healthDisplay = new GameObject { name = "Enemy Health Display" };

	//		healthDisplay.AddComponent<Canvas> ();
	//		healthDisplay.AddComponent<CanvasScaler> ();
	//		healthDisplay.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;

	//		foreach (HealthSystem enemyHealth in enemyHealths)
	//		{
	//			GameObject display = new GameObject ("Health Display");
	//			display.AddComponent<Text> ();
	//			display.GetComponent<Text> ().color = Color.white;
	//			display.GetComponent<Text> ().text = "sdsdsdsdsdsdklfslgnkdng";
	//			display.transform.SetParent (healthDisplay.transform);
	//		}
	//	}
	//}

	//private void Update ()
	//{
	//	if (assignDoorHandler)
	//	{
	//		assignDoorHandler = false;

	//		DoorHandler[] handlers = FindObjectsOfType<DoorHandler> ();

	//		if (handlers.Length == 0) Debug.LogWarning ("No Door Handlers found.");
	//		else if (handlers.Length > 1) Debug.LogWarning ("More than 1 Door Handler in scene.");
	//		else doorHandler = handlers[0];
	//	}
	//}

#endif
}