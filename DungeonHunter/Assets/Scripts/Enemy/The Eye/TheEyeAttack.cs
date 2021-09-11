using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FiredProjectile
{
	public GameObject eyeObject;
	public Vector3 direction;
	public float flightTime;
	public ProjectileTrigger trigger;
}

public class TheEyeAttack : MonoBehaviour
{
	[Header ("Components:")]

	public Transform eyeTransform;
	public GameObject projectilePrefab;

	public Transform player;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem eyeHealth;

	[SerializeField]
	private HealthSystem playerHealth;

	[SerializeField]
	private DoorHandler doorHandler;

	[Header ("Variables:")]

	private float projectileSpeed = 4f;
	private int projectileDamage = 8;
	private float attackCoolDown = 3f;

	private FiredProjectile[] projectiles;

	[Header ("Scripts:")]

	public TheEyeAnimator eyeAnimator;

	// Enumerators

	private IEnumerator eyeAttackSequence { get { return EyeAttackSequence (); } }
	private IEnumerator projectileFlight { get { return ProjectileFlight (); } }

	// !!! TEMPORARY !!!
	private void Start () { Execute (); }

	public void Execute ()
	{
		StartCoroutine (eyeAttackSequence);
		StartCoroutine (projectileFlight);
	}

	private IEnumerator EyeAttackSequence ()
	{
		while (!eyeHealth.Dead ())
		{
			for (float coolDown = attackCoolDown; coolDown > 0; coolDown -= Time.deltaTime) yield return null; // Cool Down

			yield return ProjectileAttack ();
		}

		// Let Player Exit

		doorHandler.Unlock ();

		// Kill Eye

		gameObject.SetActive (false);
	}

	private IEnumerator ProjectileAttack ()
	{
		eyeAnimator.Set_AttackStart ();

		while (!eyeAnimator.Is_AttackStartDone ()) yield return null; // Lead in Attack Animation

		// Spawn Projectile

		GameObject newProjectile = Instantiate (projectilePrefab, eyeTransform.position, Quaternion.identity);

		// Charge Projectile

		for (float timer = 2f; timer > 0; timer -= Time.deltaTime) yield return null;

		// Fire Projectile

		Vector3 dir = Vector3.Normalize (player.position - eyeTransform.position);

		// Add to Fired Projectile Array

		if (projectiles == null || projectiles.Length <= 0)
		{
			FiredProjectile[] newProjectiles = new FiredProjectile[1];

			newProjectiles[0] = new FiredProjectile
			{
				eyeObject = newProjectile,
				direction = dir,
				flightTime = 4f,
				trigger = newProjectile.GetComponent<ProjectileTrigger> ()
			};

			projectiles = newProjectiles;
		}

		FiredProjectile[] tempProjectiles = new FiredProjectile[projectiles.Length + 1];
		projectiles.CopyTo (tempProjectiles, 0);

		tempProjectiles[projectiles.Length - 1] = new FiredProjectile { 
			eyeObject = newProjectile, 
			direction = dir, flightTime = 4f, 
			trigger = newProjectile.GetComponent<ProjectileTrigger> () };

		projectiles = tempProjectiles;

		// Lead Out Attack Animation

		eyeAnimator.Set_AttackExiting ();
	}

	private IEnumerator ProjectileFlight ()
	{
		while (enabled)
		{
			while (projectiles != null)
			{
				for (int i = 0; i < projectiles.Length; i++)
				{
					if (projectiles[i].flightTime > 0)
					{
						projectiles[i].flightTime -= Time.deltaTime;

						if (projectiles[i].trigger.triggered)
						{
							// Damage Player

							playerHealth.Damage (projectileDamage);

							// Destroy this Projectile

							DestroyProjectile (projectiles[i]);
						}
						else
						{
							projectiles[i].eyeObject.transform.position += projectiles[i].direction * projectileSpeed * Time.deltaTime;
						}
					}
					else
					{
						DestroyProjectile (projectiles[i]);
					}
				}

				yield return null;
			}

			yield return null;
		}
	}

	private void DestroyProjectile (FiredProjectile fp)
	{
		List<FiredProjectile> tempProjectiles = new List<FiredProjectile> ();
		tempProjectiles.AddRange (projectiles);
		tempProjectiles.Remove (fp);
		projectiles = tempProjectiles.ToArray ();

		Destroy (fp.eyeObject);
	}
}
