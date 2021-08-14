using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
	[Header ("Components:")]

	public SpriteRenderer itemSR;

	[SerializeField]
	private Sprite goldSprite;
	[SerializeField]
	private Sprite potionSprite;

	// Variables

	private float timer;

	public void FlashByItemType (ItemType type)
	{
		switch (type)
		{
			case ItemType.Gold:
				FlashIcon (goldSprite);
				break;
			case ItemType.PotionOfHealth:
				FlashIcon (potionSprite);
				break;
		}
	}

    private void FlashIcon (Sprite itemSprite)
	{
		itemSR.sprite = itemSprite;
		timer = 1f;

		if (!gameObject.activeSelf) gameObject.SetActive (true);
	}

	private void Update ()
	{
		if (timer > 0)
		{
			timer -= Time.deltaTime;
			return;
		}

		gameObject.SetActive (false);
	}
}
