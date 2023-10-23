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

	// Audio

	[SerializeField]
	private AudioSource _audioSource;
	[SerializeField]
	private AudioClip _batIdle_Sound;
	[SerializeField]
	private AudioClip _bat_WindUp_Sound;
	[SerializeField]
	private AudioClip _death_Sound;

	[Header ("Scripts:")]

	[SerializeField]
	private BatAnimatorFunctions batAnim;

	// Variables

	private float speed = 1f;

	private bool attacking;
	private bool swoop;
	private float swoopStopThresh = 0.5f;

	private Vector3 atkStartPos;
	private Vector3 attackDir;
	private Vector3 atkEndPos;

	// Enumerators

	private IEnumerator BatSeq { get { return DoBatSeq (); } }

	[Header ("Debug:")]

	[SerializeField]
	private SpriteRenderer debugSR;

	public void Execute () { StartCoroutine (BatSeq); }

	private IEnumerator DoBatSeq ()
	{
		batCollider.enabled = false;

		while (enabled)
		{
			if (!healthSystem.Is_Dead ())
			{
				if (!attacking)
				{
					if (Vector3.Magnitude (master.position - target.position) > 1)
					{
						// Move

						if (!_audioSource.isPlaying)
							_audioSource.PlayOneShot (_batIdle_Sound);

						MoveToTarget (target.position);
					}
					else
					{
						attacking = true; // Player found, start Attack

						atkStartPos = master.position;
						attackDir = Vector3.Normalize (target.position - master.position);
						atkEndPos = atkStartPos + attackDir * 2f;
					}
				}
				else
				{
					if (!swoop)
					{
#if UNITY_EDITOR
						if (debugSR) debugSR.color = Color.blue;
#endif

						// Wind Up

						if (Vector3.Magnitude (master.position - atkStartPos) < 0.75f)
						{
							// Wind Up Sound

							if (!_audioSource.isPlaying)
								_audioSource.PlayOneShot (_bat_WindUp_Sound);

							master.Translate (-attackDir * speed * Time.deltaTime);
						}
						else
						{
							swoop = true;

							batCollider.enabled = true;
						}
					}
					else
					{
#if UNITY_EDITOR
						if (debugSR) debugSR.color = Color.red;
#endif

						if (Vector3.Magnitude (master.position - atkEndPos) > swoopStopThresh)
						{
							if (batCollider.triggered)
							{
#if UNITY_EDITOR
								if (debugSR) debugSR.color = Color.white;
#endif

								batCollider.triggered = false;

								batCollider.enabled = false; // Disable Attack

								// Damage Player

								targetHealthSystem.GetHurt ();
							}

							Vector3 newPos = Vector3.Lerp (master.position, atkEndPos, speed * 3 * Time.deltaTime);

							master.position = newPos;
						}
						else
						{
#if UNITY_EDITOR
							if (debugSR) debugSR.color = Color.white;
#endif

							attacking = false;
							swoop = false;

							atkStartPos = new Vector3 (0, 0, 0);
							attackDir = new Vector3 (0, 0, 0);
							atkEndPos = new Vector3 (0, 0, 0);

							if (batCollider.enabled) batCollider.enabled = false; // Disable Attack

							for (float coolDown = 2f; coolDown > 0; coolDown -= Time.deltaTime)
							{
								if (healthSystem.Is_Dead ()) batAnim.Set_Dead (true);

								yield return null;
							}
						}
					}
				}
			}
			else
			{
				// Death Sound

				if (_death_Sound)
					_audioSource.PlayOneShot (_death_Sound, 0.5f);

				// Death Animation

				batAnim.Set_Dead (true);
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