using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public IEnumerator Attack ()
	{
		spriteRenderer.sprite = attackSprite;

		attackCollider.enabled = true;

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
			collision.GetComponent<HealthSystem> ().Damage (currDamage);
		}
	}
}
