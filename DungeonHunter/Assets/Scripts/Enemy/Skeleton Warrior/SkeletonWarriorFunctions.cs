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
			// Distance to Target

			float gapToTarg = Vector3.Magnitude (master.position - target.position);

			if(gapToTarg > attackRange)
			{
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
		
		//while (!healthSystem.Dead () && !targetHealthSystem.Dead ())
		//{
			// Distance to Target

			//float gapToTarg = Vector3.Magnitude (master.position - target.position);

			// Chase

			// If Blocked by Pillar, Go Around

			//if (goAround.Blocked (goAround.master.position, goAround.target.position))
			//{

			//}

			// If Not Blocked, Run at Target

			//if(gapToTarg > attackRange)
			//{
			//	Vector3 moveValue = Vector3.Normalize (target.position - master.position);
			//	moveValue.z = 0;

			//	master.position += moveValue * moveSpeed * Time.deltaTime;
			//}
			//else
			//{
			//	yield return DoAttackSeq (); // Attack
			//}

			//yield return null;
		//}

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
		// Wind Up

		for (float windupTimer = windUp; windupTimer > 0; windupTimer -= Time.deltaTime)
		{
			if (!healthSystem.Dead ())  yield return null;
		}

		// Attack Animation

		skeletonAnimFunctions.Set_AttackStart ();

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
