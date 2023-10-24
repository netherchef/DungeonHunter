using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatBeholder : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject lazer;

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

	[Header ("Scripts:")]

	[SerializeField]
	private GreatBeholderAnimatorFunctions animFuncs;

	[SerializeField]
	private LazerAttack lazerAttack;

	[SerializeField]
	private SceneBounds sceneBounds;

	[Header ("Death Components:")]

	[SerializeField]
	private CircleCollider2D beholderCollider;

	[SerializeField]
	private Canvas healthCanvas;

	// Variables

	private float moveSpeed = 1f;

	private bool teleporting;

	// Enumerators

	private IEnumerator currentAction;

	public IEnumerator Attack (Transform beholder, Transform targ, HealthSystem hs)
	{
		animFuncs.Set_Attacking (true);

		// Attacks

		yield return LazerGaze (targ, beholder, hs);

		// if (hs.currHp < hs.fullHp / 2) yield return TrackerGaze (beholder, targ);
		// else yield return LazerGaze (targ, beholder);

		animFuncs.Set_Attacking (false);
	}

	#region Misc _______________________________________________________________

	private IEnumerator Move (Transform beholder, Transform targ, float dur = 0)
	{
		if (dur > 0)
		{
			for (float timer = dur; timer > 0; timer -= Time.deltaTime)
			{
				Do_Move (beholder, targ);

				yield return null;
			}
		}
		else
		{
			while (enabled)
			{
				Do_Move (beholder, targ);

				yield return null;
			}
		}
	}

	private void Do_Move (Transform beholder, Transform targ)
	{
		Vector3 movement = Vector3.Normalize (targ.position - beholder.position);
		movement.y = 0;

		beholder.Translate (movement * moveSpeed * Time.deltaTime);
	}

	public void LookAtPlayer (Transform pupil, Transform pupilCont, Transform targ)
	{
		pupil.position = pupilCont.position + LookAtPlayer_PupilPos (pupilCont, targ) * 0.075f;
	}

	private Vector3 LookAtPlayer_PupilPos (Transform pupilCont, Transform targ)
	{
		Vector3 dirToTarg = targ.position - pupilCont.position;
		return dirToTarg.normalized;
	}

	public void TeleportRandom (Transform trans, HealthSystem hs)
	{
		float dice = Random.Range (0, 4);
			
		Vector3 pos = new Vector3 ();

		if (dice == 0) pos = sceneBounds.TopRight () + new Vector3 (-0.5f, -0.5f);
		else if (dice == 1) pos = sceneBounds.BtmRight () + new Vector3 (-0.5f, 0.5f);
		else if (dice == 2) pos = sceneBounds.BtmLeft () + new Vector3 (0.5f, 0.5f);
		else if (dice == 3) pos = sceneBounds.TopLeft () + new Vector3 (0.5f, -0.5f);

		Teleport (transform, pos, hs);
	}

	private void Teleport (Transform trans, Vector3 targPos, HealthSystem hs)
	{
		if (currentAction != null)
		{
			StopCoroutine (currentAction);
			currentAction = null;
		}
		
		currentAction = DoTeleport (trans, targPos, hs);
		StartCoroutine (currentAction);

		teleporting = true;
	}

	private IEnumerator DoTeleport (Transform trans, Vector3 targPos, HealthSystem hs)
	{
		float speed = 2f;

		while (trans.localScale.y > 0 && !hs.Is_Dead ())
		{
			Vector3 newScale = trans.localScale - new Vector3 (speed, speed, 0) * Time.deltaTime;

			if (newScale.y >= 0)
			{
				trans.localScale = newScale;
			}
			else
			{
				trans.localScale = new Vector3 (0,0,trans.localScale.z);
			}

			yield return null;
		}

		SnapTransToPos (trans, targPos);

		while (trans.localScale.y < 1 && !hs.Is_Dead ())
		{
			Vector3 newScale = trans.localScale + new Vector3 (speed, speed, 0) * Time.deltaTime;

			if (newScale.y < 1) trans.localScale = newScale;
			else trans.localScale = new Vector3 (1, 1, trans.localScale.z);

			yield return null;
		}

		trans.localScale = new Vector3 (1, 1, trans.localScale.z);

		teleporting = false;
	}

	public bool Teleporting () { return teleporting; }

	private void SnapTransToPos (Transform trans, Vector3 targPos)
	{
		trans.position = targPos;
	}

	#endregion

	#region Attack _____________________________________________________________

	private IEnumerator LazerGaze (Transform pTrans, Transform gbTrans, HealthSystem hs)
	{
		//for (float chargeDur = 1f; chargeDur > 0; chargeDur -= Time.deltaTime) // Charge
		//{
		//	if(hs.Dead ()) chargeDur = 0;

		//	yield return null;
		//}

		// Lazer Rotation

		Vector3 vectorToTarget = pTrans.position - gbTrans.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		lazer.transform.rotation = q;

		// Lazer Length

		float newLength = lazer.transform.localScale.y * Vector3.Magnitude (pTrans.position - gbTrans.position) * 3.5f;
		Vector3 newScale = new Vector3 (lazer.transform.localScale.x, newLength, lazer.transform.localScale.z);
		lazer.transform.localScale = newScale;

		// Charge Sound

		Stop_All_Sounds ();
		Play_Sound (_beamChargeAudioSource);

		for (float chargeDur = 1f; chargeDur > 0; chargeDur -= Time.deltaTime) // Charge
		{
			if (hs.Is_Dead ()) chargeDur = 0;

			yield return null;
		}

		// Shoot Sound

		Stop_All_Sounds ();
		Play_Sound (_beamShootAudioSource);

		lazer.SetActive (true); // Release

		for (float duration = 3; duration > 0; duration -= Time.deltaTime)
		{
			if(hs.Is_Dead ()) duration = 0;

			yield return null;
		}

		lazer.SetActive (false); // Stop

		lazer.transform.localScale = new Vector3 (1, 1, 1); // Reset Lazer Size

		if (lazerAttack.LazerHit ()) lazerAttack.Reset (); // Reset Lazer Attack
	}

	#endregion

	#region Death ______________________________________________________________

	public void Start_Death ()
	{
		StartCoroutine ("Death");
	}

	private IEnumerator Death ()
	{
		// Death Sound

		Stop_All_Sounds ();
		Play_Sound (_deathSound);

		beholderCollider.enabled = false;
		healthCanvas.gameObject.SetActive (false);

		animFuncs.Set_Dead (true);

		yield return null;
	}

	#endregion

	private void Play_Sound (AudioSource src)
	{
		if (!src.isPlaying)
		{
			src.Play ();
		}
	}

	private void Stop_All_Sounds ()
	{
		if (_idleAudioSource.isPlaying) _idleAudioSource.Stop ();
		if (_warpAudioSource.isPlaying) _warpAudioSource.Stop ();
		if (_beamChargeAudioSource.isPlaying) _beamChargeAudioSource.Stop ();
		if (_beamShootAudioSource.isPlaying) _beamShootAudioSource.Stop ();
		if (_deathSound.isPlaying) _deathSound.Stop ();
	}
}
