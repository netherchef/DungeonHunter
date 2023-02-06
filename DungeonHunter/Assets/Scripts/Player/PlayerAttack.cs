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

	[SerializeField]
	private PlayerSounds sounds;

	[Header ("Variables:")]

	[SerializeField]
	private int currDamage = 1;
	private int initDamage = 1;

	[SerializeField]
	private Attack_Effect currEffect;
	private float knockback = 0.2f;

	private Vector2 attackDir;

	[Header ("Debug:")]

	[SerializeField]
	private bool debugAttack;

	public IEnumerator Attack (Vector2 dir)
	{
		attackDir = dir;

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

				animator.Set_FacingRight (true);
				animator.Set_FacingLeft (false);
			}
			else if (dir.x < 0) // Left
			{
				tempRot.z = 0;

				animator.Set_FacingLeft (true);
				animator.Set_FacingRight (false);
			}
		}
		else if (absDir.y > absDir.x)
		{
			if (dir.y > 0) // Up
			{
				tempRot.z = -90;

				animator.Set_FacingUp (true);
				animator.Set_FacingDown (false);
			}
			else if (dir.y < 0) // Down
			{
				tempRot.z = 90;

				animator.Set_FacingDown (true);
				animator.Set_FacingUp (false);
			}
		}

		attackCollider.transform.localRotation = Quaternion.Euler (tempRot.x, tempRot.y, tempRot.z); // Decide Attack Direction

		attackCollider.enabled = true; // Enable Collider

		// Attack Sequence

		animator.Set_Attacking (true);

		sounds.Play_SwordSwing (); // Attack Sound

		for (float timer = 0.25f; timer > 0; timer -= Time.deltaTime) yield return null;

		attackCollider.enabled = false;

		animator.Set_Attacking (false);
	}

	#region Set Damage _________________________________________________________

	public void SetDamage (int value, bool multiply = false, bool reset = false)
	{
		if (reset)
		{
			currDamage = initDamage;
			DataPasser.DPInstance.Set_CurrDamage (1);
			return;
		}

		if (multiply) currDamage *= value;
		else currDamage = value;

		DataPasser.DPInstance.Set_CurrDamage (currDamage);
	}

	public void IncrementDamage ()
	{
		currDamage++;
		DataPasser.DPInstance.Set_CurrDamage (currDamage);
	}

	public bool DamageChanged () { return currDamage != initDamage; }

	public int CurrentDamage () { return currDamage; }

	#endregion

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("AttackTarget"))
		{
			HealthSystem targHealth = collision.GetComponent<HealthSystem> ();

			if (currEffect == Attack_Effect.DOT) targHealth.ApplyDOT (1, 5); // DOT Effect

			targHealth.GetHurt (currDamage); // Basic Attack

			// Knockback

			//if (!targHealth.IsBoss ())
			//{
			//	targHealth.transform.position += new Vector3 (attackDir.x, attackDir.y) * knockback;
			//}

#if UNITY_EDITOR
			if (debugAttack)
				Debug.Log ("Damage " + targHealth.transform.name + " | " + "DMG: " + currDamage + ", " + "Final HP: " + targHealth.CurrHP ());
#endif
		}
	}

	#region Attack Effect ______________________________________________________

	public void Set_AttackEffect (Attack_Effect e) { currEffect = e; }

	public Attack_Effect CurrentAttackEffect () { return currEffect; }

	#endregion
}
