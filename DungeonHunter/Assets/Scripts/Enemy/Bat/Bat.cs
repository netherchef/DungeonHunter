using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
	[Header ("Components:")]

	public Transform master;
	public Transform body;

	[SerializeField]
	private BatCollider batCollider;
	[SerializeField]
	private HealthSystem healthSystem;

	private Transform target;
	private HealthSystem targetHealthSystem;

	[Header ("Scripts:")]

	[SerializeField]
	private BatAnimatorFunctions batAnim;

	// Variables

	private float speed = 1f;

	private bool attacking;
	private bool swoop;

	private Vector3 atkStartPos;
	private Vector3 attackDir;
	private Vector3 atkEndPos;

	// Enumerators

	private IEnumerator BatSeq { get { return DoBatSeq (); } }

	public void Execute () { StartCoroutine (BatSeq); }

	private IEnumerator DoBatSeq ()
	{
		while (enabled)
		{
			if (!healthSystem.Dead ())
			{
				if (!attacking)
				{
					if (Vector3.Magnitude (master.position - target.position) > 1)
					{
						// Move

						MoveToTarget (target.position);

						// Damage

						if (batCollider.triggered)
						{
							batCollider.triggered = false;

							targetHealthSystem.Damage ();
						}
					}
					else
					{
						attacking = true;

						atkStartPos = master.position;
						attackDir = Vector3.Normalize (target.position - master.position);
						atkEndPos = atkStartPos + attackDir * 2f;
					}
				}
				else
				{
					if (!swoop)
					{
						// Wind Up

						if (Vector3.Magnitude (master.position - atkStartPos) < 0.75f)
						{
							master.Translate (-attackDir * speed * Time.deltaTime);
						}
						else
						{
							swoop = true;
						}
					}
					else
					{
						if (Vector3.Magnitude (master.position - atkEndPos) > 0.1f)
						{
							Vector3 newPos = Vector3.Lerp (master.position, atkEndPos, speed * 3 * Time.deltaTime);

							master.position = newPos;
						}
						else
						{
							attacking = false;
							swoop = false;

							atkStartPos = new Vector3 (0, 0, 0);
							attackDir = new Vector3 (0, 0, 0);
							atkEndPos = new Vector3 (0, 0, 0);
						}
					}
				}
			}
			else
			{
				// Death

				batAnim.Set_Dead ();
			}

			yield return null;
		}

		//while (healthSystem.currHp > 0 && !targetHealthSystem.Dead ())
		//while (!healthSystem.Dead ())
		//{
		//	if (Vector3.Magnitude (master.position - target.position) > 1)
		//	{
		//		// Move

		//		MoveToTarget (target.position);

		//		// Damage

		//		if (batCollider.triggered)
		//		{
		//			batCollider.triggered = false;

		//			targetHealthSystem.Damage ();
		//		}
		//	}
		//	else
		//	{

		//	}

		//	yield return null;
		//}

		//// Death

		//batAnim.Set_Dead ();
	}

	public void MoveToTarget (Vector3 targPos)
	{
		master.Translate (Vector3.Normalize (targPos - master.position) * speed * Time.deltaTime);
	}

	public HealthSystem HealthSystem () { return healthSystem; }

	#region Spawn Functions ____________________________________________________

	public void SetTarget (Transform targTrans)
	{
		target = targTrans;
	}

	public void Set_TargetHealth (HealthSystem healthSys)
	{
		targetHealthSystem = healthSys;
	}

	#endregion
}