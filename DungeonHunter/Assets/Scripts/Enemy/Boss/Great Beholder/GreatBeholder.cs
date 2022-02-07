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
	private SceneBounds sceneBounds;

	// Variables

	private float moveSpeed = 1f;

	// Enumerators

	private IEnumerator currentAction;

	public IEnumerator Misc (Transform gb, Transform targ, Transform pupil, Transform pupilCont)
	{
		LookAtPlayer (pupil,pupilCont, targ);

		//yield return Move (gb, targ, 4f);

		yield return null;
	}

	public IEnumerator Attack (Transform beholder, Transform targ, HealthSystem hs)
	{
		// Attacks

		if (hs.currHp < hs.fullHp / 2) yield return TrackerGaze (beholder, targ);
		else yield return LazerGaze ();
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

	private void LookAtPlayer (Transform pupil, Transform pupilCont, Transform targ)
	{
		pupil.position = pupilCont.position + LookAtPlayer_PupilPos (pupilCont, targ) * 0.075f;
	}

	private Vector3 LookAtPlayer_PupilPos (Transform pupilCont, Transform targ)
	{
		Vector3 dirToTarg = targ.position - pupilCont.position;
		return dirToTarg.normalized;
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cancel"))
		{
			float dice = Random.Range (0, 4);
			
			Vector3 pos = new Vector3 ();

			if (dice == 0) pos = sceneBounds.TopRight ();
			else if (dice == 1) pos = sceneBounds.BtmRight ();
			else if (dice == 2) pos = sceneBounds.BtmLeft ();
			else if (dice == 3) pos = sceneBounds.TopLeft ();

			Teleport (transform, pos);
			print (pos);
		}
	}

	private void Teleport (Transform trans, Vector3 targPos)
	{
		if (currentAction != null)
		{
			StopCoroutine (currentAction);
			currentAction = null;
		}
		
		currentAction = DoTeleport (trans, targPos);

		StartCoroutine (currentAction);
	}

	private IEnumerator DoTeleport (Transform trans, Vector3 targPos)
	{
		while (trans.localScale.y > 0)
		{
			trans.localScale -= new Vector3 (1, 1, 0) * Time.deltaTime;
			yield return null;
		}

		SnapTransToPos (trans, targPos);

		while (trans.localScale.y < 1)
		{
			trans.localScale += new Vector3 (1, 1, 0) * Time.deltaTime;
			yield return null;
		}

		trans.localScale = new Vector3 (1, 1, trans.localScale.z);
	}

	private void SnapTransToPos (Transform trans, Vector3 targPos)
	{
		trans.position = targPos;
	}

	#endregion

	#region Attack _____________________________________________________________

	private IEnumerator LazerGaze ()
	{
		for (float duration = 2f; duration > 0; duration -= Time.deltaTime) yield return null; // Charge

		lazer.SetActive (true); // Release

		for (float duration = 3; duration > 0; duration -= Time.deltaTime) yield return null;

		lazer.SetActive (false); // Stop

		for (float cooldown = 4f; cooldown > 0; cooldown -= Time.deltaTime) yield return null; // Cooldown
	}

	private IEnumerator TrackerGaze (Transform beholder, Transform targ)
	{
		// Charge

		// Release

		lazer.SetActive (true);

		for (float duration = 3; duration > 0; duration -= Time.deltaTime)
		{
			Vector3 movement = Vector3.Normalize (targ.position - beholder.position);
			movement.y = 0;

			beholder.Translate (movement * (moveSpeed * 1.5f) * Time.deltaTime);

			yield return null;
		}

		// Stop

		lazer.SetActive (false);

		// Cooldown

		for (float cooldown = 4f; cooldown > 0; cooldown -= Time.deltaTime) yield return null;
	}

	#endregion

	#region Death ______________________________________________________________

	#endregion
}
