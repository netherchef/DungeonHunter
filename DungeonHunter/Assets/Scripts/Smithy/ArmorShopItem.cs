using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType { NULL, Gold, Bronze, Ruby }

public class ArmorShopItem : MonoBehaviour
{
	[SerializeField]
	private ArmorType armorType;

	private bool playerDetected;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player")) playerDetected = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag ("Player")) playerDetected = false;
	}

	public void Disable()
	{
		playerDetected = false;
	}

	public bool PlayerDetected()
	{
		return playerDetected;
	}

	public ArmorType ShopArmorType ()
	{
		return armorType;
	}
}