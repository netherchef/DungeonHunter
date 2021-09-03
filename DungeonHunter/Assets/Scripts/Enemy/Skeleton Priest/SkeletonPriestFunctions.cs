using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriestFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject skeletonPrefab;

	[Header ("Scripts:")]

	[SerializeField]
	private SkeletonPriestAnimatorFunctions animFuncs;

	[SerializeField]
	private EnemyHandler enemyHandler;

	[Header ("Variables:")]

	[SerializeField]
	private float coolDown = 1f;
	[SerializeField]
	private float castTime = 2f;

	// !!! TEMPORARY !!!
	private void OnEnable () { Execute (); }

	public void Execute ()
	{
		StartCoroutine (PriestCycle ());
	}

	private IEnumerator PriestCycle ()
	{
		while (enabled)
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
	}

    private void Summon ()
	{
		enemyHandler.SpawnAtRandomLocations (skeletonPrefab);
	}
}
