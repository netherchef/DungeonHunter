using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType { NULL, GreatBeholder, SkeleMech }

public class Boss : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform trans;
	[SerializeField]
	private Transform pTrans;

	// Audio

	[SerializeField]
	private AudioSource _idleAudioSource;
	[SerializeField]
	private AudioSource _warpAudioSource;
	[SerializeField]
	private AudioSource _beamChargeAudioSource;
	[SerializeField]
	private AudioSource _beamShootAudioSource;
	[SerializeField]
	private AudioSource _deathSound;

	//[SerializeField]
	//private Transform pupilContainer;
	//[SerializeField]
	//private Transform pupil;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem bossHealth;
	[SerializeField]
	private HealthSystem pHealth;

	[SerializeField]
	private DoorHandler doorHandler;

	[Header ("Boss Scripts:")]

	[SerializeField]
	private GreatBeholder greatBeholder;
	[SerializeField]
	private SkeleMech skeleMech;

	[Header ("Variables:")]

	[SerializeField]
	private BossType type;

	// Enumerators

	private IEnumerator bossLoop;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		Execute ();
	}

	public void Prep ()
	{
		switch (type)
		{
			case BossType.GreatBeholder:
				bossLoop = GBLoop ();
				break;
		}
	}

	public void Execute () { StartCoroutine (bossLoop); }

	private IEnumerator GBLoop ()
	{
		// Prep

		if (DataPasser.DPInstance.defeatedBosses.Contains (BossType.GreatBeholder))
		{
			// Unlock Doors

			doorHandler.Unlock ();

			// Death

			greatBeholder.Start_Death ();

			yield break;
		}

		// Intro
		
		// Main Loop

		float attackInterval = 5f;
		float delay = 0;
		bool telStart = false;

		while (!bossHealth.Is_Dead () && !pHealth.Is_Dead ())
		{
			// Idle Sound

			if (!_idleAudioSource.isPlaying && !_warpAudioSource.isPlaying)
			{
				//Stop_All_Sounds ();
				_idleAudioSource.Play ();
			}

			// TEMPORARY

			//greatBeholder.LookAtPlayer (pupil, pupilContainer, pTrans); // Misc Action

			if (attackInterval > 0)
			{
				attackInterval -= Time.deltaTime;

				// Teleport

				if (Vector2.Distance (trans.position, pTrans.position) < 1f)
				{
					if (!telStart && !greatBeholder.Teleporting ())
					{
						telStart = true;
						delay = 0.5f;
					}
					else if (telStart && !greatBeholder.Teleporting ())
					{
						if (delay <= 0) // Delay Complete
						{
							// Teleport Sound

							_warpAudioSource.PlayOneShot (_warpAudioSource.clip, 1f);

							greatBeholder.TeleportRandom (trans, bossHealth);

							telStart = false;
						}
						else
						{
							delay -= Time.deltaTime;
						}
					}
				}
			}
			else
			{
				yield return greatBeholder.Attack (trans, pTrans, bossHealth); // Attack

				attackInterval = 5f;
			}

			yield return null;
		}
		
		// Unlock Doors

		doorHandler.Unlock ();

		// Death

		if (pHealth.Is_Dead ()) { } // Player Death

		// Disabling the Boss here temporarily to allow other processes
		// before disabling it altogether.

		// Death

		greatBeholder.Start_Death ();

		DataPasser.DPInstance.RecordBossDefeat (BossType.GreatBeholder);
	}

	//private void Play_Sound (AudioSource src, float vol = 1)
	//{
	//	if (!src.isPlaying)
	//	{
	//		src.volume = vol;
	//		src.Play ();
	//		print ("Playing: " + src.name);
	//	}
	//}

	//private void Stop_All_Sounds ()
	//{
	//	if (_idleAudioSource.isPlaying) _idleAudioSource.Stop ();
	//	if (_warpAudioSource.isPlaying) _warpAudioSource.Stop ();
	//	if (_beamChargeAudioSource.isPlaying) _beamChargeAudioSource.Stop ();
	//	if (_beamShootAudioSource.isPlaying) _beamShootAudioSource.Stop ();
	//	if (_deathSound.isPlaying) _deathSound.Stop ();
	//}
}