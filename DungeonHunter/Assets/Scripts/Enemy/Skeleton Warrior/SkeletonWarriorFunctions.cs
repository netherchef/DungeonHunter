using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarriorFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[Header ("Spawn Components:")]

	private Transform target;
	private HealthSystem targetHealthSystem;
	private LootHandler lootHandler;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem healthSystem;
	[SerializeField]
	private GoAround goAround;

	[SerializeField]
	private EnemyAnimatorFunctions skeletonAnimFunctions;

	[Header ("Variables")]

	[SerializeField]
	private float attackRange = 0.6f;
	[SerializeField]
	private float moveSpeed = 1f;
	[SerializeField]
	private float windUp = 0.2f;
	[SerializeField]
	private float coolDown = 0.2f;

	// Enumerators

	private IEnumerator skeletonSeq;

	public void Execute (bool summon)
	{
		skeletonSeq = DoSkeletonSeq (summon);
		StartCoroutine (skeletonSeq);
	}

	private IEnumerator DoSkeletonSeq (bool summon)
	{
		// Wait for Summon Animation to End

		if (summon) while (!skeletonAnimFunctions.Is_SummonDone ()) yield return null;

		while (!healthSystem.Dead () && !targetHealthSystem.Dead ())
		{
			float gapToTarg = Vector3.Magnitude (master.position - target.position); // Distance to Target

			if (gapToTarg > attackRange)
			{
				// Move to Target

				Vector3 moveValue = Vector3.Normalize (target.position - master.position);
				moveValue.z = 0;

				master.position += moveValue * moveSpeed * Time.deltaTime;
			}
			else
			{
				yield return DoAttackSeq (); // Attack
			}

			yield return null;
		}

		// Death

		gameObject.SetActive (false);

		// Drop Loot

		lootHandler.DropGold (master.position);

		//ItemType[] items = new ItemType[1];
		//items[0] = ItemType.Gold;

		//lootHandler.DropLoot (items, master.position);
	}

	private IEnumerator DoAttackSeq ()
	{
		// Set Direction for Animation

		float dir = Mathf.Sign (target.position.x - master.position.x);

		skeletonAnimFunctions.Set_FacingRight (dir <= 0 ? false : true);

		// Start Attack Animation

		skeletonAnimFunctions.Set_AttackStart ();

		// Wind Up

		while (!skeletonAnimFunctions.Is_WindUpDone ()) yield return null;

		skeletonAnimFunctions.Set_WindUpDone_False ();

		// Attack

		bool foundTarget = false;

		while (!skeletonAnimFunctions.Is_AttackDone () && !healthSystem.Dead ())
		{
			if (!foundTarget)
			{
				if (Vector3.Magnitude (target.position - master.position) < attackRange)
				{
					foundTarget = true;
					targetHealthSystem.GetHurt ();
				}
			}

			yield return null;
		}

		// Cooldown

		for (float cdTimer = coolDown; cdTimer > 0; cdTimer -= Time.deltaTime)
		{
			if(!healthSystem.Dead ()) yield return null;
		}
	}

	public HealthSystem HealthSystem () { return healthSystem; }

	#region Spawn Functions ____________________________________________________

	public void SetTarget (Transform targTrans)
	{
		target = targTrans;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealthSystem = healthSys;
	}

	public void Set_LootHandler (LootHandler handler)
	{
		lootHandler = handler;
	}

	public void Set_GoAround (Transform playerTrans)
	{
		goAround.target = playerTrans;
	}

	public void Set_SummonStart ()
	{
		skeletonAnimFunctions.Set_SummonStart ();
	}

	#endregion
}
