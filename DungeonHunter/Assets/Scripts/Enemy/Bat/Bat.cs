using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
	[Header ("Components:")]

	public Transform bodyHolder;
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

	// Enumerators

	private IEnumerator BatSeq { get { return DoBatSeq (); } }

	public void Execute () { StartCoroutine (BatSeq); }

	private IEnumerator DoBatSeq ()
	{
		//while (healthSystem.currHp > 0 && !targetHealthSystem.Dead ())
		while (!healthSystem.Dead ())
		{
			// Move

			MoveToPlayer (target.position);

			// Damage

			if (batCollider.triggered)
			{
				batCollider.triggered = false;

				targetHealthSystem.Damage ();
			}

			yield return null;
		}

		// Death

		batAnim.Set_Dead ();
	}

	public void MoveToPlayer (Vector3 playerPos)
	{
		bodyHolder.Translate (Vector3.Normalize (playerPos - bodyHolder.position) * speed * Time.deltaTime);
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