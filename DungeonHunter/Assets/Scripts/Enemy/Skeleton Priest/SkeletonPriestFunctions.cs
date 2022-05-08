using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriestFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;
	private Transform target;

	[SerializeField]
	private GameObject arcanceOrb;

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
		bool summonDone = false;
		bool attackDone = false;

		bool summon = false;
		bool attack = false;
		bool panic = false;

		while (!healthSystem.Dead () && !targetHealthSystem.Dead ())
		{
			// Wait Cool Down

			for (float cd = coolDown; cd > 0; cd -= Time.deltaTime)
			{
				if (healthSystem.Dead ()) cd = 0;

				yield return null;
			}

			// Decide Next Action

			if (!summonDone)
			{
				summon = true;
			}
			else
			{
				if (!attackDone) attack = true;
				else panic = true;
			}			

			if (attack) // Arcane Orb
			{
				attack = false;

				attackDone = true;

				animFuncs.Set_SummonStart ();

				GameObject orb = null;

				if (!healthSystem.Dead ())
				{
					orb = Instantiate (arcanceOrb, master.position, Quaternion.identity);
					orb.transform.position += new Vector3 (0, 0.5f);
				}

				for (float timer = 3f; timer > 0; timer -= Time.deltaTime)
				{
					if (healthSystem.Dead ()) timer = 0;
					else yield return null;
				}

				if (!healthSystem.Dead ())
				{
					animFuncs.Set_SummonDone ();

					Vector2 dir = target.position - orb.transform.position;
					orb.GetComponent<ArcaneOrbShot> ().Shoot (dir.normalized);
				}
			}
			else if (summon) // Summon Skeletons
			{
				summon = false;

				summonDone = true;

				animFuncs.Set_SummonStart ();

				for (float timer = 3f; timer > 0; timer -= Time.deltaTime)
				{
					if (healthSystem.Dead ()) timer = 0;
					else yield return null;
				}

				animFuncs.Set_SummonDone ();

				if (!healthSystem.Dead ()) SummonSkeleton (); // Spawn Skeleton
			}
			else if (panic) // Panic
			{
				panic = false;
				summonDone = false;
				attackDone = false;

				animFuncs.Set_Panic (true);

				for (float timer = 3f; timer > 0; timer -= Time.deltaTime)
				{
					if (healthSystem.Dead ()) timer = 0;
					else yield return null;
				}

				animFuncs.Set_Panic (false);
			}

			yield return null;
		}

		// Dead

		animFuncs.Set_Dead ();

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

	public void SetTarget(Transform targTrans)
	{
		target = targTrans;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealthSystem = healthSys;
	}

	#endregion
}