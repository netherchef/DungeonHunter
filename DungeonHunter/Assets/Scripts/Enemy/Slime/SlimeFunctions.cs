using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[SerializeField]
	private CircleCollider2D attackCollider;

	[SerializeField]
	private Transform target;

	// Audio

	[SerializeField]
	private AudioSource _audioSource;
	[SerializeField]
	private AudioClip _slime_Move_Sound;
	[SerializeField]
	private AudioClip _slime_Jump_Sound;
	[SerializeField]
	private AudioClip _slime_Land_Sound;
	[SerializeField]
	private AudioClip _slime_Burst_Sound;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem health;

	[SerializeField]
	private SlimeAnimatorFunctions animFunctions;

	[SerializeField]
	private JumpArch jumpArch;

	private SceneBounds sceneBounds;

	private HealthSystem targetHealth;
	private CircleCollider2D targCol;

	private LootHandler lootHandler;

	// Variables

	private float speed = 0.5f;

	// Enumerators

	private IEnumerator SlimeSeq { get { return DoSlimeSeq (); } }

	public void Execute () { StartCoroutine (SlimeSeq); }

	private IEnumerator DoSlimeSeq ()
	{
		while (!health.Is_Dead ())
		{
			if (!health.Is_Dead ())
			{
				if (Vector3.Distance (target.position, master.position) > 1f) // If far away
				{
					// Slime Move Sound

					if (!_audioSource.isPlaying)
						_audioSource.PlayOneShot (_slime_Move_Sound);

					Vector3 dir = Vector3.Normalize (target.position - master.position);

					Vector3 newPos = sceneBounds.ClampPointInBounds (master.position + (dir * speed * Time.deltaTime));

					master.position = newPos;
				}
				else
				{
					for (float windUp = 1f; windUp > 0; windUp -= Time.deltaTime) yield return null;

					// Jump Sound

					_audioSource.PlayOneShot (_slime_Jump_Sound);

					// Jump

					jumpArch.Jump (master.position, target.position, 4, 0.5f);

					while (jumpArch.Is_Jumping ()) yield return null;

					// Landing Sound

					_audioSource.PlayOneShot (_slime_Land_Sound);

					// Impact On Landing

					if (!health.Is_Dead ())
					{
						attackCollider.enabled = true;

						for (float attackDur = 0.25f; attackDur > 0; attackDur -= Time.deltaTime)
						{
							if (!health.Is_Dead ())
							{
								if (attackCollider.IsTouching (targCol))
								{
									targetHealth.GetHurt ();

									attackCollider.enabled = false; // Disable Attack

									attackDur = 0;
								}
							}
							else
							{
								attackDur = 0;
							}

							yield return null;
						}

						attackCollider.enabled = false; // Disable Attack

						for (float coolDown = 2; coolDown > 0; coolDown -= Time.deltaTime) // Cooldown
						{
							if (health.Is_Dead ()) coolDown = 0;

							yield return null;
						}
					}
				}
			}

			yield return null;
		}

		// Slime Death Sound

		_audioSource.PlayOneShot (_slime_Burst_Sound);

		// Death

		animFunctions.Set_Dead (true); // Start Death Animation

		while (animFunctions.Is_Dead ()) yield return null; // Wait for Death Animation to End.

		lootHandler.DropGold (master.position);
	}

	public HealthSystem HealthSystem () { return health; }

	public void Set_TargetTransform (Transform trans)
	{
		target = trans;
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

	public void Set_SceneBounds (SceneBounds sb)
	{
		sceneBounds = sb;
	}
}