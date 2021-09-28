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

	[Header ("Scripts:")]

	[SerializeField]
	private PlayerAnimator animator;

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
			if (dir.x > 0) // Right
			{
				tempRot.z = 180;

				animator.Set_FacingRight ();
			}
			else if (dir.x < 0) // Left
			{
				tempRot.z = 0;

				animator.Set_FacingLeft ();
			}
		}
		else if (absDir.y > absDir.x)
		{
			if (dir.y > 0) // Up
			{
				tempRot.z = -90;

				animator.Set_FacingUp ();
			}
			else if (dir.y < 0) // Down
			{
				tempRot.z = 90;

				animator.Set_FacingDown ();
			}
		}

		attackCollider.transform.localRotation = Quaternion.Euler (tempRot.x, tempRot.y, tempRot.z); // Decide Attack Direction

		attackCollider.enabled = true; // Enable Collider

		// Attack Sequence

		animator.Set_Attacking (true);

		for (float timer = 0.25f; timer > 0; timer -= Time.deltaTime) yield return null;

		attackCollider.enabled = false;

		animator.Set_Attacking (false);

		//bool done = false;

		//float animTimer = animDuration;
		//float colTimer = colDuration;

		//while (!done)
		//{
		//	if (animTimer > 0) animTimer -= Time.deltaTime;

		//	if (colTimer > 0) colTimer -= Time.deltaTime;
		//	else attackCollider.enabled = false;

		//	if (animTimer <= 0 && colTimer <= 0) done = true;

		//	yield return null;
		//}

		//spriteRenderer.sprite = initSprite;
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
			HealthSystem targHealth = collision.GetComponent<HealthSystem> ();

			if (currEffect == Attack_Effect.DOT)
			{
				// Apply DOT Effect

				targHealth.ApplyDOT (1, 5);

				// Basic Attack

				targHealth.Damage (currDamage);
			}
			else
			{
				// Basic Attack

				targHealth.Damage (currDamage);

				Debug.Log ("Damage " + targHealth.transform.name + " | " + "DMG: " + currDamage + ", " + "Final HP: " + targHealth.CurrHP ());
			}
		}
	}

	#region Attack Effect ______________________________________________________

	public void Set_AttackEffect (Attack_Effect e) { currEffect = e; }

	public Attack_Effect CurrentAttackEffect () { return currEffect; }

	#endregion
}
