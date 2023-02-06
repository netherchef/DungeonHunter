using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
	// Components

	[SerializeReference]
	private GameObject replacementPotion;

	// Variables

	[SerializeField]
	private ItemType type;
	[SerializeField]
	private int cost = 1;

	[SerializeField]
	private AudioClip itemSound;

	private bool ready;

	private void Start ()
	{
		if (type == ItemType.PotionOfStrength)
		{
			// Disable Damage Upgrade if already taken

			if (DataPasser.DPInstance.DamageUpgraded ())
			{
				replacementPotion.SetActive (true);
				gameObject.SetActive (false);
			}
		}
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = true;
	}

	private void OnTriggerExit2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) ready = false;
	}

	public bool Ready () { return ready; }

	public void Disable () { ready = false; }

	public ItemType Type () { return type; }

	public int Cost () { return cost; }

	public void PlayItemSound (AudioSource audioS)
	{
		if (itemSound != null) audioS.PlayOneShot (itemSound);
	}
}