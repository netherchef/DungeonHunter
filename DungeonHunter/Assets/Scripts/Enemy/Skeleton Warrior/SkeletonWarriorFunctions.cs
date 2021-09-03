using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarriorFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;
	[SerializeField]
	private Transform target;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem healthSystem;

	private HealthSystem targetHealthSystem;

	public LootHandler lootHandler;

	public EnemyAnimatorFunctions enemyAnimatorFunctions;

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

	private IEnumerator SkeletonSeq { get { return DoSkeletonSeq (); } }
	//private IEnumerator AttackSeq { get { return DoAttackSeq (); } }

	public void Execute ()
	{
		StartCoroutine (SkeletonSeq);
		//StartCoroutine (AttackSeq);
	}

	private IEnumerator DoSkeletonSeq ()
	{
		while (enabled)
		{
			if (!healthSystem.Dead ())
			{
				// Distance to Target

				float gapToTarg = Vector3.Magnitude (master.position - target.position);

				// Chase

				if (gapToTarg > attackRange)
				{
					Vector3 moveValue = Vector3.Normalize (target.position - master.position);
					moveValue.z = 0;

					master.position += moveValue * moveSpeed * Time.deltaTime;
				}
				else
				{
					yield return DoAttackSeq (); // Attack
				}

				// Check Attack

				//yield return DoAttackSeq ();

				//if (gapToTarg < attackRange)
				//{
				//if (!attack) attack = true;

				//	yield return DoAttackSeq ();
				//}

				//Remove Health

				//if (damageTarget)
				//{
				//	damageTarget = false;

				//	targetHealthSystem.Damage ();
				//}
			}
			else
			{
				// Death

				if (gameObject.activeSelf)
				{
					gameObject.SetActive (false);

					// Drop Loot

					ItemType[] items = new ItemType[1];
					items[0] = ItemType.Gold;

					lootHandler.DropLoot (items, master.position);
				}
			}

			yield return null;
		}
	}

	private IEnumerator DoAttackSeq ()
	{
		// Wind Up

		for (float windupTimer = windUp; windupTimer > 0; windupTimer -= Time.deltaTime) yield return null;

		// Attack Animation

		enemyAnimatorFunctions.AttackStart ();

		// Attack

		bool foundTarget = false;

		while (!enemyAnimatorFunctions.Is_AttackDone ())
		{
			if (!foundTarget)
			{
				if (Vector3.Magnitude (target.position - master.position) < attackRange)
				{
					foundTarget = true;
					//damageTarget = true;
					targetHealthSystem.Damage ();
				}
			}

			yield return null;
		}

		// Cooldown

		for (float cdTimer = coolDown; cdTimer > 0; cdTimer -= Time.deltaTime) yield return null;
	}

	public void SetTarget (Transform targTrans)
	{
		target = targTrans;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealthSystem = healthSys;
	}
}
