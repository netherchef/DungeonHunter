using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibility : MonoBehaviour
{
	[Header ("Components:")]

	public SpriteRenderer playerSpriteRenderer;

	[Header ("Variables:")]

	public bool invincible;

	private IEnumerator doInvincibility;

	public void GoInvincible ()
	{
		doInvincibility = DoInvincibility ();
		StartCoroutine (doInvincibility);
	}

    public IEnumerator DoInvincibility ()
	{
		invincible = true;

		float flashTime = 0.2f;

		int blinks = 10;

		BlinkSprite ();

		while (blinks > 0)
		{
			if (flashTime > 0)
			{
				flashTime -= Time.deltaTime;
			}
			else
			{
				blinks--;
				BlinkSprite ();
				flashTime = 0.2f;
			}

			yield return null;
		}

		Color tempCol = playerSpriteRenderer.color;
		tempCol.a = 1;
		playerSpriteRenderer.color = tempCol;

		invincible = false;
	}

	private void BlinkSprite ()
	{
		Color tempCol = playerSpriteRenderer.color;
		tempCol.a *= -1;
		playerSpriteRenderer.color = tempCol;
	}
}
