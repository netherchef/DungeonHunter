using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attack_Effect { NULL, DOT }

public class PlayerAttack : MonoBehaviour
{
	[Header ("Components:")]

	public SpriteRenderer spriteRenderer;

	public Sprite initSprite;
	public Sprite attackSprite;

	public BoxCollider2D attackCollider;

	[Header ("Variables:")]

	[SerializeField]
	private int currDamage = 1;
	private int initDamage = 1;

	private float animDuration = 0.25f;
	private float colDuration = 0.1f;

	[SerializeField]
	private Attack_Effect currEffect;

	public IEnumerator Attack (Vector2 dir)
	{
		// Animate

		spriteRenderer.sprite = attackSprite;

		// Prep Collider

		Vector3 tempRot = attackCollider.transform.localRotation.eulerAngles;

		Vector2 absDir = new Vector2 (Mathf.Abs (dir.x), Mathf.Abs (dir.y));

		if (absDir.x > absDir.y)
		{
			if (dir.x > 0) tempRot.z = 180;
			else if (dir.x < 0) tempRot.z = 0;
		}
		else if (absDir.y > absDir.x)
		{
			if (dir.y > 0) tempRot.z = -90;
			else if (dir.y < 0) tempRot.z = 90;
		}

		attackCollider.transform.localRotation = Quaternion.Euler (tempRot.x, tempRot.y, tempRot.z);

		attackCollider.enabled = true;

		// Attack Sequence

		bool done = false;

		float animTimer = animDuration;
		float colTimer = colDuration;

		while (!done)
		{
			if (animTimer > 0) animTimer -= Time.deltaTime;

			if (colTimer > 0) colTimer -= Time.deltaTime;
			else attackCollider.enabled = false;

			if (animTimer <= 0 && colTimer <= 0) done = true;

			yield return null;
		}

		spriteRenderer.sprite = initSprite;
	}

	#region Set Damage _________________________________________________________

	public void SetDamage (int value, bool multiply = false, bool reset = false)
	{
		if (reset)
		{
			currDamage = initDamage;
			return;
		}

		if (multiply) currDamage *= value;
		else currDamage = value;
	}

	public bool DamageChanged () { return currDamage != initDamage; }

	public int CurrentDamage () { return currDamage; }

	#endregion

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("AttackTarget"))
		{
			// Attack Effects

			if (currEffect == Attack_Effect.DOT)
			{
				HealthSystem targHealth = collision.GetComponent<HealthSystem> ();

				// Apply DOT Effect

				targHealth.ApplyDOT (1, 5);

				// Basic Attack

				targHealth.Damage (currDamage);
			}
			else
			{
				// Basic Attack

				collision.GetComponent<HealthSystem> ().Damage (currDamage);
			}
		}
	}

	#region Attack Effect ______________________________________________________

	public void Set_AttackEffect (Attack_Effect e) { currEffect = e; }

	public Attack_Effect CurrentAttackEffect () { return currEffect; }

	#endregion
}
