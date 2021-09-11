﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientGuardFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[SerializeField]
	private BoxCollider2D attackCollider;

	[SerializeField]
	private SpriteRenderer sr;

	// Target

	private Transform targetTrans;
	private HealthSystem targetHealth;
	private CircleCollider2D targCol;
	private LootHandler lootHandler;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem healthSystem;

	// Variables

	private int damage = 4;
	private float moveSpeed = 0.5f;

	//private bool attacking;
	//private bool targFound;

	// Enumerators

	private IEnumerator AG;

	public void Execute ()
	{
		AG = DoAG ();
		StartCoroutine (AG);
	}

	private IEnumerator DoAG ()
	{
		while (!healthSystem.Dead ())
		{
			// Move

			while (Vector3.Magnitude (master.position - targetTrans.position) > 0.5f)
			{
				Move ();

				yield return null;
			}

			// Charge Up

			sr.color = Color.red;

			for (float chargeTime = 2f; chargeTime > 0; chargeTime -= Time.deltaTime) yield return null;

			// Attack

			sr.color = Color.white;

			yield return Attack ();

			// Cool Down

			for (float cooldown = 1f; cooldown > 0; cooldown -= Time.deltaTime) yield return null;

			yield return null;
		}

		lootHandler.DropGold (master.position);

		gameObject.SetActive (false);
	}

	private void Move ()
	{
		Vector3 direction = Vector3.Normalize (targetTrans.position - master.position);

		master.position += direction * moveSpeed * Time.deltaTime;
	}

	private IEnumerator Attack ()
	{
		bool attacking = true;

		attackCollider.enabled = true;

		float attackDur = 0.1f;

		while (attacking)
		{
			if (attackDur > 0) attackDur -= Time.deltaTime;
			else attacking = false;

			if (attackCollider.IsTouching (targCol))
			{
				attacking = false;

				targetHealth.Damage (damage);
			}

			yield return null;
		}

		attackCollider.enabled = false;
	}

	//private void OnTriggerEnter2D (Collider2D collision)
	//{
	//	if (collision.CompareTag ("Player"))
	//	{
	//		if (attacking) targFound = true;
	//	}
	//}

	#region Spawn Functions ____________________________________________________

	public void Set_TargetTransform (Transform trans)
	{
		targetTrans = trans;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealth = healthSys;
	}

	public void Set_TargetCollider (CircleCollider2D col)
	{
		targCol = col;
	}

	public void Set_LootHandler (LootHandler handler)
	{
		lootHandler = handler;
	}

	public HealthSystem HealthSystem () { return healthSystem; }

	#endregion
}