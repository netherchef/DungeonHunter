using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Enemy
{
	// Enemy Info

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

	[Header ("Scripts:")]

	public HealthSystem targetHealthSystem;
	public LootHandler lootHandler;

	private Enemy[] enemies;

	// Enumerators

	private IEnumerator checkEnemies;

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep ()
	{
		// Initialise Enemies

		enemies = new Enemy[transform.childCount];

		for (int e = 0; e < enemies.Length; e++)
		{
			// Enemy Info

			enemies[e].enemyInfo = transform.GetChild (e).GetComponent<EnemyInfo> ();

			// Health

			enemies[e].healthSystem = transform.GetChild (e).GetComponent<HealthSystem> ();

			// Enumerator

			enemies[e].attackCoroutine = AttackCoroutine (e);
			StartCoroutine (enemies[e].attackCoroutine);
		}

		// Begin

		checkEnemies = CheckEnemies ();
		StartCoroutine (checkEnemies);
	}

	private IEnumerator CheckEnemies ()
	{
		while (enabled)
		{
			for (int c = 0; c < enemies.Length; c++)
			{
				if (targetHealthSystem.currHp > 0)
				{
					Transform enemy = transform.GetChild (c);

					if (!enemies[c].healthSystem.Dead ())
					{
						// Chase

						if (!enemies[c].attack)
						{
							if (Vector3.Magnitude (enemy.position - target.position) > enemies[c].enemyInfo.attackRange)
							{
								Vector3 moveValue = -(enemy.position - target.position);
								moveValue.z = 0;

								if (Mathf.Abs (moveValue.x) > Mathf.Abs (moveValue.y))
								{
									float x = Mathf.Sign (moveValue.x);
									float y = Mathf.Clamp (Mathf.Sign (moveValue.y) * Mathf.Abs (moveValue.y / moveValue.x), -1, 1);
									moveValue = new Vector3 (x, y);
								}
								else if (Mathf.Abs (moveValue.x) < Mathf.Abs (moveValue.y))
								{
									float x = Mathf.Clamp (Mathf.Sign (moveValue.x) * Mathf.Abs (moveValue.x / moveValue.y), -1, 1);
									float y = Mathf.Sign (moveValue.y);
									moveValue = new Vector3 (x, y);
								}

								enemy.position += moveValue * enemies[c].enemyInfo.moveSpeed * Time.deltaTime;
							}
						}

						// Check Attack

						if (Vector3.Magnitude (enemy.position - target.position) < enemies[c].enemyInfo.attackRange)
						{
							if (!enemies[c].attack) enemies[c].attack = true;
						}

						// Remove Health

						if (enemies[c].damageTarget)
						{
							enemies[c].damageTarget = false;

							targetHealthSystem.Damage ();
						}
					}
					else
					{
						// Death

						if (enemy.gameObject.activeSelf)
						{
							enemy.gameObject.SetActive (false);

							// Drop Loot

							ItemType[] items = new ItemType[1];
							items[0] = ItemType.Gold;

							lootHandler.DropLoot (items, enemy.transform.position);
						}
					}
				}

				yield return null;
			}

			yield return null;
		}
	}

	private IEnumerator AttackCoroutine (int index)
	{
		while (enemies[index].healthSystem.currHp > 0)
		{
			if (enemies[index].attack)
			{
				EnemyInfo info = enemies[index].enemyInfo;

				// Wind Up

				for (float windupTimer = info.windUp; windupTimer > 0; windupTimer -= Time.deltaTime) yield return null;

				// Attack Animation

				info.enemyAnimatorFunctions.AttackStart ();

				// Attack

				bool foundTarget = false;

				while (!info.enemyAnimatorFunctions.Is_AttackDone ())
				{
					if (!foundTarget)
					{
						if (Vector3.Magnitude (target.position - transform.GetChild (index).position) < info.attackRange)
						{
							foundTarget = true;
							enemies[index].damageTarget = true;
						}
					}

					yield return null;
				}

				// Cooldown

				for (float cdTimer = info.coolDown; cdTimer > 0; cdTimer -= Time.deltaTime) yield return null;

				// Reset

				enemies[index].attack = false;
			}

			yield return null;
		}
	}
}