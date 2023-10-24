using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientGuardFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[SerializeField]
	private CircleCollider2D attackCollider;

	[SerializeField]
	private SpriteRenderer sr;

	// Target

	private Transform targetTrans;
	private HealthSystem targetHealth;
	private CircleCollider2D targCol;
	private LootHandler lootHandler;

	// Audio

	[SerializeField]
	private AudioSource _audioSource;
	[SerializeField]
	private AudioClip _axeSwingSound;
	[SerializeField]
	private AudioClip _step_Sound;
	[SerializeField]
	private AudioClip _metal_Scatter;

	[Header ("Variables:")]

	[SerializeField]
	private ItemType[] itemDrops;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem healthSystem;

	[SerializeField]
	private AncientGuardAnimatorFunctions animatorFunctions;

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
		while (!healthSystem.Is_Dead ())
		{
			// Move

			if (Vector3.Magnitude (master.position - targetTrans.position) > 0.5f && !healthSystem.Is_Dead ()) // Walking Animation
			{
				animatorFunctions.Set_Walking (true);
			}

			while (Vector3.Magnitude (master.position - targetTrans.position) > 0.5f && !healthSystem.Is_Dead ())
			{
				Move ();

				yield return null;
			}

			animatorFunctions.Set_Walking (false);

			// Charge Up

			animatorFunctions.Set_Attacking (true); // Start Attack Animation

			for (float chargeTime = 2f; chargeTime > 0; chargeTime -= Time.deltaTime)
			{
				if (healthSystem.Is_Dead ()) chargeTime = 0;

				yield return null;
			}

			// Attack

			animatorFunctions.Set_Release (); // Animate Attack Release

			if (!healthSystem.Is_Dead ()) yield return Attack ();

			// Cool Down

			for (float cooldown = 1f; cooldown > 0; cooldown -= Time.deltaTime)
			{
				if (healthSystem.Is_Dead ()) cooldown = 0;

				yield return null;
			}

			animatorFunctions.Set_Attacking (false); // Exit Attack Animation

			yield return null;
		}

		// Death Sound

		_audioSource.PlayOneShot (_metal_Scatter);

		lootHandler.DropLoot (itemDrops, master.position);

		animatorFunctions.Set_Dead (true); // Death Animation
	}

	private void Move ()
	{
		Vector3 direction = Vector3.Normalize (targetTrans.position - master.position);

		master.position += direction * moveSpeed * Time.deltaTime;
	}

	private IEnumerator Attack ()
	{
		// Axe Swing Sound

		_audioSource.PlayOneShot (_axeSwingSound, 4.0f);

		attackCollider.enabled = true;

		for (float a = 0.1f; a > 0; a -= Time.deltaTime)
		{
			if (!healthSystem.Is_Dead ())
			{
				if(attackCollider.IsTouching (targCol))
				{
					targetHealth.GetHurt (damage);
				}
			}
			else
			{
				a = 0;
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