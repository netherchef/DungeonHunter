using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem pHealthSys;

	[Header ("Variables:")]

	[SerializeField]
	private int damage;

	[SerializeField]
	private bool preserve;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) pHealthSys.GetHurt (damage);

		if (!preserve) gameObject.SetActive (false);
	}
}