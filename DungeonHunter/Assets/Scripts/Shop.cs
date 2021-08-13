﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SaleItem
{
	public ItemType type;
	public int goldCost;
	public ShopItem shopItem;
}

public class Shop : MonoBehaviour
{
	[Header ("Components:")]

	public Transform shopItemContainer;

	[Header ("Scripts:")]

	public PlayerInventory inventory;
	public ItemIcon itemIcon;

	// Variables

	private List<SaleItem> saleItems = new List<SaleItem> ();

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep ()
	{
		foreach (Transform child in shopItemContainer)
		{
			ShopItem shopItem = child.GetComponent<ShopItem> ();

			saleItems.Add (new SaleItem {
				type = shopItem.type,
				goldCost = 1,
				shopItem = shopItem
			});
		}
	}

	public void MakePurchase ()
	{
		// Get Shop Item

		SaleItem currItem = default;

		foreach (SaleItem saleItem in saleItems)
		{
			if (saleItem.shopItem.ready) currItem = saleItem;
		}

		if (currItem.type == ItemType.NULL) return;

		// Check Gold

		if (inventory.GoldCount () < currItem.goldCost) return;

		// Deduct Gold

		for (int i = currItem.goldCost; i > 0; i--)
		{
			inventory.RemoveItem (ItemType.Gold);
		}

		// Make Sale

		currItem.shopItem.ready = false;
		currItem.shopItem.gameObject.SetActive (false);

		// Add to Inventory

		inventory.AddItem (currItem.type);
		itemIcon.FlashByItemType (currItem.type);

		// Remove Item from Shop

		saleItems.Remove (currItem);
	}
}
