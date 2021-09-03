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
		while (!healthSystem.Dead ())
		{
			// Wait Cool Down

			for (float cd = coolDown; cd > 0; cd -= Time.deltaTime) yield return null;

			// Summon

			animFuncs.Set_PrepPray ();

			for (float ct = castTime; ct > 0; ct -= Time.deltaTime) yield return null;

			animFuncs.Set_DonePray ();

			// Spawn Skeleton

			Summon ();

			// Reset

			yield return null;
		}

		gameObject.SetActive (false);

		lootHandler.DropGold (master.position);
	}

    private void Summon ()
	{
		enemyHandler.Spawn_SkeletonWarrior ();
	}

	public void Set_EnemyHandler (EnemyHandler handler)
	{
		enemyHandler = handler;
	}

	public void Set_LootHandler (LootHandler handler)
	{
		lootHandler = handler;
	}
}
