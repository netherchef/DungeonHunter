using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Normal, Player }

public class HealthSystem : MonoBehaviour
{
	[Header ("Optional Components:")]

	public GameObject master;

	[Header ("Generic Variables:")]

	public UnitType type;
	public int currHp = 4;
	public bool defaultDeath = true;

	[Header ("Player:")]

	public PlayerInvincibility invincibility;
	public HealthBar healthBar;
	public PlayerDeath playerDeath;

	public int fullHp = 4;

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep ()
	{
		if (type != UnitType.Player) currHp = fullHp;
	}

	public void Damage (int value = 1)
	{
		DecreaseHP (value);
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
		if (type == UnitType.Player)
		{
			if (!invincibility.invincible)
			{
				// Decrease HP

				currHp -= value;

				// Health Bar

				healthBar.DrainHeart ();

				// Invincibility or Death

				if (currHp > 0)
				{
					invincibility.GoInvincible ();
				}
				else
				{
					playerDeath.ShowDeathScreen ();
				}
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

	public bool Dead ()
	{
		return currHp <= 0;
	}
}
