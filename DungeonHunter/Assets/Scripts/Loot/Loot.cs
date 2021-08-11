using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
	public ItemType type;

	public bool collected;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
		{
			collected = true;
			//gameObject.SetActive (false);
		}
	}
}