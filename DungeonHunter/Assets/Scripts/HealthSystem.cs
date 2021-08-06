using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	[Header ("Optional Components:")]

	public GameObject master;

	[Header ("Variables:")]

	public int hp = 10;

	public void Damage (int value = 1)
	{
		DecreaseHP (value);
	}

	private void DecreaseHP (int value)
	{
		hp -= value;

		if (master && hp <= 0)
		{
			master.SetActive (false);
		}
	}

	public bool Dead ()
	{
		return hp <= 0;
	}
}
