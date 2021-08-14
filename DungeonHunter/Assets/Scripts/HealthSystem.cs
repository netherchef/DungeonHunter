using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Normal, Player }

public class HealthSystem : MonoBehaviour
{
	[Header ("Optional Components:")]

	public GameObject master;

	public PlayerInvincibility invincibility;

	[Header ("Variables:")]

	public UnitType type;
	public int hp = 10;
	public bool defaultDeath = true;

	public void Damage (int value = 1)
	{
		DecreaseHP (value);
	}

	private void DecreaseHP (int value)
	{
		if (type == UnitType.Player)
		{
			if (!invincibility.invincible)
			{
				hp -= value;

				invincibility.GoInvincible ();
			}
		}
		else
		{
			hp -= value;
		}

		// Death

		if (defaultDeath)
		{
			if (master && hp <= 0)
			{
				master.SetActive (false);
			}
		}
	}

	public bool Dead ()
	{
		return hp <= 0;
	}
}
