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

	public void GetHurt (int value = 1)
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

	public void GetHealed (int val = 1)
	{
		if (currHp < fullHp)
		{
			if (currHp + val > fullHp) currHp = fullHp;
			else currHp += val;

			healthBar.FillHeart ();
		}
	}

	private void DecreaseHP (int damage)
	{
		if (debug) print ("Ouch!");

		if (type == UnitType.Player)
		{
			if (!invincibility.invincible)
			{
				switch (DataPasser.DPInstance.CurrentArmorType ())
				{
					case ArmorType.Gold:
						print ("Damage = " + damage + " - 1 = " + (damage - 1));
						damage -= 1;
						break;
					case ArmorType.Bronze:
						damage /= 2;
						break;
					case ArmorType.Ruby:
						damage = 1;
						break;
				}

				currHp -= damage; // Decrease HP

#if UNITY_EDITOR
				Debug.Log (this.gameObject.name + " damaged for " + damage + " | HP: " + currHp);
#endif

				if (damage > 0) healthBar.DrainHeart (); // Health Bar

				// Invincibility or Death

				if (currHp > 0) invincibility.GoInvincible ();
				else playerDeath.StartDeath ();

				camShaker.Shake (0.2f, 2f); // Camera Shake
			}
		}
		else
		{
			currHp -= damage;
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
}