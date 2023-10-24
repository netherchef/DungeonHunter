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

	[SerializeField]
	private SpriteRenderer[] characterSpriteRenderers;

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

	// Audio

	[SerializeField]
	private PlayerSounds _playerSounds;

	[SerializeField]
	private AudioSource _hurtAudioSource;

	[Header ("Boss:")]

	[SerializeField]
	private bool boss;
	[SerializeField]
	private UnityEngine.UI.Image bossHealthBar;

	// Enumerators

	private IEnumerator DOTSeq;

	private IEnumerator colourSprite;
	private float colTimer;

	// private IEnumerator doPaintWhite;

	[Header ("Debug:")]

	[SerializeField]
	private bool debug;
	[SerializeField]
	private bool godMode;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
	}

	public void Prep ()
	{
		if (type != UnitType.Player) // If Enemy
		{
			int loopCount = DataPasser.DPInstance.loopCount;

			if (loopCount <= 0)
			{
				currHp = fullHp;
				return;
			}
			else
			{
				currHp = fullHp;

				for (int i = 0; i < loopCount; i++) currHp++;

				transform.localScale += new Vector3 (1,1,1) * ((float) loopCount * 1.25f); // BIG ENEMIES!
			}
		}

		// For painting sprite white.
		// shaderGUIText = Shader.Find ("GUI/Text Shader");
		// shaderSpriteDefault = Shader.Find ("Sprites/Default");
	}

	public void GetHurt (int value = 1, bool ignoreInvin = false)
	{
#if UNITY_EDITOR
		if (godMode) return;
#endif
		if (Is_Dead ()) return;

		// Hurt Sound

		if (_hurtAudioSource && !Is_Dead ())
		{
			if (_hurtAudioSource.isPlaying)
				_hurtAudioSource.Stop ();

			_hurtAudioSource.PlayOneShot (_hurtAudioSource.clip, 0.5f);
		}

		// Player gets Hurt

		if (type == UnitType.Player)
		{
			if (DataPasser.DPInstance.GodMode ()) return;

			DecreaseHP (value + DataPasser.DPInstance.loopCount, true);

			return;
		}

		// Enemy gets Hurt

		DecreaseHP (value);

		// PaintWhite (masterSprite);

		if (boss)
		{
			// Boss Health Bar

			float newVal = (1 / (float)fullHp) * (float)currHp;
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

	private void DecreaseHP (int damage, bool ignoreInvin = false)
	{
		if (debug) print ("Ouch!");

		if (type == UnitType.Player)
		{
			if (!invincibility.invincible || ignoreInvin)
			{
				switch (DataPasser.DPInstance.CurrentArmorType ())
				{
					case ArmorType.Gold:
						damage -= 1;
						break;
					case ArmorType.Bronze:
						if (damage >= 2) damage -= 2;
						else damage = 0;
						break;
					case ArmorType.Ruby:
						if (damage >= 4) damage -= 4;
						else damage = 0;
						break;
				}

				currHp -= damage; // Decrease HP

#if UNITY_EDITOR
				//Debug.Log (this.gameObject.name + " damaged for " + damage + " | HP: " + currHp);
#endif

				if (damage > 0) healthBar.DrainHeart (); // Health Bar

				// Invincibility or Death

				if (currHp > 0) invincibility.GoInvincible ();
				else
				{
					// Player Death Sound

					_playerSounds.Play_DeathSound ();

					playerDeath.StartDeath ();
				}

				camShaker.Shake (0.2f, 2f); // Camera Shake
			}
		}
		else
		{
			currHp -= damage;

			// Change to Damaged Sprite Colour

			//if (characterSpriteRenderers[0] == null) return;
			if (characterSpriteRenderers == null) return;

			colTimer = 1f;

			if (colourSprite == null)
			{
				colourSprite = ColourSprite ();
				StartCoroutine (colourSprite);
			}
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

	public bool Is_Dead () { return currHp <= 0; }

	public int CurrHP () { return currHp; }

	private IEnumerator ColourSprite ()
	{
		foreach (SpriteRenderer sr in characterSpriteRenderers)
		{
			sr.color = Color.red;
		}

		while (colTimer > 0)
		{
			colTimer -= Time.deltaTime;

			yield return null;
		}

		foreach (SpriteRenderer sr in characterSpriteRenderers)
		{
			sr.color = Color.white;
		}

		colourSprite = null;
	}

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

	public void Toggle_GodMode ()
	{
		godMode = !godMode;
	}
}