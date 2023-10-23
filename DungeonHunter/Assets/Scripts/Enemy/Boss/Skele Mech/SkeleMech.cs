using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeleMech : MonoBehaviour
{
	// Components

	[SerializeField]
	private Transform skeleTrans;

	[SerializeField]
	private GameObject fireball;

	[SerializeField]
	private Canvas bossHealthBar;

	[SerializeField]
	private Collider2D[] colliders;

	// Audio

	[SerializeField]
	private AudioSource _groundSlamAudioSource;
	[SerializeField]
	private float _landing_Volume = 10f;
	[SerializeField]
	private float _groundSlam_Volume = 5f;

	[SerializeField]
	private AudioSource _breathAudioSource;
	[SerializeField]
	private AudioSource _fireStartAudioSource;

	[SerializeField]
	private AudioSource _deathAudioSource;
	[SerializeField]
	private float _deathSound_Volume = 2f;

	// Scripts

	[SerializeField]
	private HealthSystem skelHealth;

	[SerializeField]
	private SkeleMechAnim skeleMechAnim;

	[SerializeField]
	private DoorHandler doorHandler;

	[SerializeField]
	private CameraShaker camShaker;

	[SerializeField]
	private CinematicBars cineBars;

	[SerializeField]
	private DemoEnding demoEndingHandler;

	// Variables

	[SerializeField]
	private Vector2 startPos;
	[SerializeField]
	private Vector2 endPos;

	// Enumerators

	private IEnumerator SkeleLoop { get { return do_SkeleLoop (); } }

	private void OnEnable () { StartCoroutine (SkeleLoop); }

	private IEnumerator do_SkeleLoop ()
	{
		if (DataPasser.DPInstance.defeatedBosses.Contains (BossType.SkeleMech))
		{
			// Death

			skeleMechAnim.Death (); // Death Animation

			// Hide Health Bar

			bossHealthBar.gameObject.SetActive (false);

			// Disable Colliders

			Disable_Colliders ();

			// Unlock Doors

			doorHandler.Unlock ();

			yield break;
		}

		// Intro

		yield return Intro ();

		yield return Idle (2f);

		while (!skelHealth.Is_Dead ())
		{
			// Attack

			float n = Random.Range (0, 2);
			
			if (Mathf.Ceil (n) < 1) yield return GroundSlam ();
			if (Mathf.Ceil (n) >= 1) yield return Fireball ();

			// Idle

			yield return Idle (2f);
		}

		// Death Sound

		_deathAudioSource.PlayOneShot (_deathAudioSource.clip, _deathSound_Volume);

		// Death

		skeleMechAnim.Death (); // Death Animation

		// Hide Health Bar

		bossHealthBar.gameObject.SetActive (false);

		// Disable Colliders

		Disable_Colliders ();

		// Record Boss Defeat
		// Disabled since this is the last Boss of the Dungeon

		//DataPasser.DPInstance.RecordBossDefeat (BossType.SkeleMech);

		// Wait 2 Seconds

		for (float timer = 2f; timer > 0; timer -= Time.deltaTime) yield return null;

		// Display Ending Message

		if (DataPasser.DPInstance.loopCount <= 0)
		{
			yield return demoEndingHandler.ShowEndingMessage ();
		}

		// Unlock Doors

		doorHandler.Unlock ();

		//!!! TEMPORARY !!!
		//gameObject.SetActive (false);
	}

	private IEnumerator Intro ()
	{
		// Show Cinematic Bars

		cineBars.ShowCinematicBars ();

		// Boss Drops Down

		skeleTrans.position = new Vector3 (startPos.x, startPos.y, skeleTrans.position.z);

		// Moving Down

		while (skeleTrans.position.y > endPos.y)
		{
			Vector3 tempPos = skeleTrans.position;
			tempPos += new Vector3 (0, -0.5f * Time.deltaTime, 0);
			skeleTrans.position = tempPos;

			yield return null;
		}

		skeleTrans.position = new Vector3 (endPos.x, endPos.y, skeleTrans.position.z);

		// Landing Sound

		_groundSlamAudioSource.PlayOneShot (_groundSlamAudioSource.clip, _landing_Volume);

		// Camera Shake on Landing

		camShaker.Shake (0.5f, 0.5f);

		// Hide Cinematic Bars

		cineBars.End ();

		yield return null;
	}

	private IEnumerator GroundSlam ()
	{
		skeleMechAnim.GS_Wind ();

		for (float timer = 2f; timer > 0; timer -= Time.deltaTime)
		{
			yield return null;
		}

		// Ground Slam Audio

		_groundSlamAudioSource.PlayOneShot (_groundSlamAudioSource.clip, _groundSlam_Volume);

		// Ground Slam Animation

		skeleMechAnim.GroundSlam ();
	}

	private IEnumerator Fireball ()
	{
		// Windup Sound

		_breathAudioSource.PlayOneShot (_breathAudioSource.clip);

		// Fireball Windup Animation

		skeleMechAnim.Fireball_Wind ();

		for (float timer = 0; timer < 2f; timer += Time.deltaTime)
		{
			if (skelHealth.Is_Dead ()) timer = 2f; // Death

			yield return null;
		}

		// Fireball Sound

		_fireStartAudioSource.PlayOneShot (_fireStartAudioSource.clip);

		Spawn_Fireball ();

		skeleMechAnim.Fireball_End ();
	}

	private void Spawn_Fireball ()
	{
		GameObject ball = Instantiate (fireball, transform.position, Quaternion.identity);

		if (!ball.activeSelf) ball.SetActive (true);
	}

	private IEnumerator Idle (float duration)
	{
		for (float timer = duration; timer > 0; timer -= Time.deltaTime)
		{
			if (skelHealth.Is_Dead ()) timer = 0; // Death

			yield return null;
		}
	}

	private void Disable_Colliders ()
	{
		foreach (Collider2D collider in colliders)
		{
			collider.enabled = false;
		}
	}
}