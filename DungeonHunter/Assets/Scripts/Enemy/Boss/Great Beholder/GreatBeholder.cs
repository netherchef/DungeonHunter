using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatBeholder : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject lazer;

	[Header ("Scripts:")]

	[SerializeField]
	private GreatBeholderAnimation gBAnim;

	[SerializeField]
	private LazerAttack lazerAttack;

	[SerializeField]
	private SceneBounds sceneBounds;

	// Variables

	private float moveSpeed = 1f;

	private bool teleporting;

	// Enumerators

	private IEnumerator currentAction;

	// public IEnumerator Misc (Transform gb, Transform targ, Transform pupil, Transform pupilCont)
	// {
	// 	LookAtPlayer (pupil,pupilCont, targ);

	// 	//yield return Move (gb, targ, 4f);

	// 	yield return null;
	// }

	public IEnumerator Attack (Transform beholder, Transform targ, HealthSystem hs)
	{
		// Attacks

		yield return LazerGaze (targ, beholder, hs);

		// if (hs.currHp < hs.fullHp / 2) yield return TrackerGaze (beholder, targ);
		// else yield return LazerGaze (targ, beholder);
	}

	public IEnumerator Death ()
	{
		yield return null;
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

		while (trans.localScale.y > 0 && !hs.Dead ())
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

		while (trans.localScale.y < 1 && !hs.Dead ())
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
		for (float chargeDur = 2f; chargeDur > 0; chargeDur -= Time.deltaTime) // Charge
		{
			if(hs.Dead ()) chargeDur = 0;

			yield return null;
		}

		// Lazer Rotation

		Vector3 vectorToTarget = pTrans.position - gbTrans.position;
		float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		lazer.transform.rotation = q;

		// Lazer Length

		float newLength = lazer.transform.localScale.y * Vector3.Magnitude (pTrans.position - gbTrans.position) * 3.5f;
		Vector3 newScale = new Vector3 (lazer.transform.localScale.x, newLength, lazer.transform.localScale.z);
		lazer.transform.localScale = newScale;

		lazer.SetActive (true); // Release

		for (float duration = 3; duration > 0; duration -= Time.deltaTime)
		{
			if(hs.Dead ()) duration = 0;

			yield return null;
		}

		lazer.SetActive (false); // Stop

		lazer.transform.localScale = new Vector3 (1, 1, 1); // Reset Lazer Size

		if (lazerAttack.LazerHit ()) lazerAttack.Reset (); // Reset Lazer Attack
	}

	#endregion

	#region Death ______________________________________________________________

	#endregion
}
