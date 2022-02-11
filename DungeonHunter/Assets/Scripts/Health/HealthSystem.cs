using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Normal, Player }

public class HealthSystem : MonoBehaviour
{
	[Header ("Optional Components:")]

	public GameObject master;
	// [SerializeField]
	// private SpriteRenderer masterSprite;
	// private Shader shaderGUIText;
	// private Shader shaderSpriteDefault;

	[SerializeField]
	private CameraShaker camShaker;

	[Header ("Generic Variables:")]

	public UnitType type;
	public int currHp = 4;
	public int fullHp = 4;
	public bool defaultDeath = true;

	private Color initCol;
	private float whiteTimer;

	[Space (10)]

	[SerializeField]
	private int DOTDamage;
	[SerializeField]
	private float DOTDuration;

	[Header ("Player:")]

	public PlayerInvincibility invincibility;
	public HealthBar healthBar;
	public PlayerDeath playerDeath;

	[Header ("Boss:")]

	[SerializeField]
	private bool boss;
	[SerializeField]
	private UnityEngine.UI.Image bossHealthBar;

	// Enumerators

	private IEnumerator DOTSeq;

	// private IEnumerator doPaintWhite;

	[Header ("Debug:")]

	[SerializeField]
	private bool debug;
	[SerializeField]
	private bool godMode;

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep ()
	{
		if (type != UnitType.Player) currHp = fullHp; // Set Current HP

		// For painting sprite white.
		// shaderGUIText = Shader.Find ("GUI/Text Shader");
		// shaderSpriteDefault = Shader.Find ("Sprites/Default");
	}

	public void Damage (int value = 1)
	{
#if UNITY_EDITOR
		if (godMode) return;
#endif

		DecreaseHP (value);

		// PaintWhite (masterSprite);

		if (boss)
		{
			// Boss Health Bar

			float newVal = (1 / (float) fullHp) * (float) currHp;
			bossHealthBar.rectTransform.localScale = new Vector2 (newVal, bossHealthBar.rectTransform.localScale.y);
		}
	}

	public void Heal (int val = 1)
	{
		if (currHp < fullHp)
		{
			currHp += val;

			healthBar.FillHeart ();
		}
	}

	private void DecreaseHP (int value)
	{
		if (debug) print ("Ouch!");

		if (type == UnitType.Player)
		{
			if (!invincibility.invincible)
			{
				currHp -= value; // Decrease HP

				healthBar.DrainHeart (); // Health Bar

				// Invincibility or Death

				if (currHp > 0) invincibility.GoInvincible ();
				else playerDeath.StartDeath ();

				camShaker.Shake (0.2f, 2f); // Camera Shake
			}
		}
		else
		{
			currHp -= value;
		}

		// Death

		if (defaultDeath)
		{
			if (master && currHp <= 0)
			{
				master.SetActive (false);
			}
		}
	}

	public bool Dead () { return currHp <= 0; }

	public int CurrHP () { return currHp; }

	#region DOT ________________________________________________________________

	private IEnumerator DoDOTSeq ()
	{
		float wait = 1f;

		while (DOTDuration > 0)
		{
			DOTDuration -= Time.deltaTime;

			if (wait > 0)
			{
				wait -= Time.deltaTime;
			}
			else
			{
				DecreaseHP (DOTDamage);

				wait = 1f;
			}

			yield return null;
		}
	}

	public void ApplyDOT (int damage, float duration)
	{
		DOTDamage = damage;
		DOTDuration = duration;

		if (DOTSeq == null)
		{
			//affectedBy = Attack_Effect.DOT;

			DOTSeq = DoDOTSeq ();
			StartCoroutine (DOTSeq);
		}
	}

	#endregion

	#region Boss __________________________________________________

	public bool IsBoss () { return boss; }

	#endregion __________________________________________________

	#region Sprite Colour __________________________________________________

	// private void PaintWhite (SpriteRenderer sr)
	// {	
	// 	if (doPaintWhite == null)
	// 	{
	// 		doPaintWhite = DoPaintWhite (sr);
	// 		StartCoroutine (doPaintWhite);
	// 	}

	// 	whiteTimer = 0.5f;
	// }

	// private IEnumerator DoPaintWhite (SpriteRenderer sr)
	// {
	// 	masterSprite.material.shader = shaderGUIText;
	// 	masterSprite.color = Color.white;

	// 	while (whiteTimer > 0)
	// 	{
	// 		whiteTimer -= Time.deltaTime;
	// 		yield return null;
	// 	}

	// 	masterSprite.material.shader = shaderSpriteDefault;
	// 	masterSprite.color = Color.white;

	// 	doPaintWhite = null;
	// }

	#endregion __________________________________________________
}