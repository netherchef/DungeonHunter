using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriestFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[Header ("Scripts:")]

	[SerializeField]
	private SkeletonPriestAnimatorFunctions animFuncs;

	[SerializeField]
	private EnemyHandler enemyHandler;

	[SerializeField]
	private HealthSystem healthSystem;

	private LootHandler lootHandler;

	[SerializeField]
	private HealthSystem targetHealthSystem;

	[Header ("Variables:")]

	[SerializeField]
	private float coolDown = 1f;
	[SerializeField]
	private float castTime = 2f;

	public void Execute ()
	{
		StartCoroutine (PriestCycle ());
	}

	private IEnumerator PriestCycle ()
	{
		while (!healthSystem.Dead () && !targetHealthSystem.Dead ())
		{
			// Wait Cool Down

			for (float cd = coolDown; cd > 0; cd -= Time.deltaTime) yield return null;

			// Summon

			animFuncs.Set_PrepPray ();

			for (float ct = castTime; ct > 0; ct -= Time.deltaTime) yield return null;

			animFuncs.Set_DonePray ();

			// Spawn Skeleton

			SummonSkeleton ();

			// Reset

			yield return null;
		}

		gameObject.SetActive (false);

		lootHandler.DropGold (master.position);
	}

    private void SummonSkeleton ()
	{
		enemyHandler.Spawn_SkeletonWarrior (true);
	}

	public HealthSystem HealthSystem () { return healthSystem; }

	#region Spawn Functions ____________________________________________________

	public void Set_EnemyHandler (EnemyHandler handler)
	{
		enemyHandler = handler;
	}

	public void Set_LootHandler (LootHandler handler)
	{
		lootHandler = handler;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealthSystem = healthSys;
	}

	#endregion
}