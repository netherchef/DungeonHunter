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

	// Variables

	private float moveSpeed = 1f;

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

	#endregion

	#region Attack _____________________________________________________________

	private IEnumerator LazerGaze ()
	{
		// Charge

		for (float duration = 2f; duration > 0; duration -= Time.deltaTime) yield return null;

		// Release

		lazer.SetActive (true);

		for (float duration = 3; duration > 0; duration -= Time.deltaTime) yield return null;

		// Stop

		lazer.SetActive (false);

		// Cooldown

		for (float cooldown = 4f; cooldown > 0; cooldown -= Time.deltaTime) yield return null;
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
