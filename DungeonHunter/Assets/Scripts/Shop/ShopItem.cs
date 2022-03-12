using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
	// Variables

	[SerializeField]
	private ItemType type;
	[SerializeField]
	private int cost = 1;

	private bool ready;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = true;
	}

	private void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = false;
	}

	public bool Ready ()
	{
		return ready;
	}

	public void Disable ()
	{
		ready = false;
	}

	public ItemType Type ()
	{
		return type;
	}

	public int Cost ()
	{
		return cost;
	}
}