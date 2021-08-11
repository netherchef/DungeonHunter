using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
	public ItemType type;

	public bool ready;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = true;
	}

	private void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = false;
	}
}
