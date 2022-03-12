using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct SaleItem
//{
//	public ItemType type;
//	public int goldCost;
//	public ShopItem shopItem;
//}

public class Shop : MonoBehaviour
{
	[Header ("Components:")]

	public Transform shopItemContainer;

	[Header ("Scripts:")]

	public HealthSystem playerHealth;
	public PlayerAttack playerAttack;
	public PlayerInventory inventory;
	public ItemIcon itemIcon;

	// Variables

	private List<ShopItem> shopItems = new List<ShopItem> ();

	// !!! TEMPORARY !!!
	private void Start () { Prep (); }

	public void Prep ()
	{
		foreach (Transform child in shopItemContainer)
		{
			shopItems.Add (child.GetComponent<ShopItem> ());

			//ShopItem shopItem = child.GetComponent<ShopItem> ();

			//shopItems.Add (new SaleItem {
			//	type = shopItem.type,
			//	goldCost = 1,
			//	shopItem = shopItem
			//});
		}
	}

	public void MakePurchase ()
	{
		// Locate Intended Shop Item in List

		ShopItem currShopItem = null;

		foreach (ShopItem shopItem in shopItems)
		{
			if (shopItem.Ready ())
			{
				currShopItem = shopItem;
			}
		}

		if (currShopItem == null) return;
		
		//SaleItem currItem = default;

		//foreach (SaleItem saleItem in shopItems)
		//{
		//	if (saleItem.shopItem.ready) currItem = saleItem;
		//}

		if (currShopItem.Type () == ItemType.NULL)
		{
			Debug.LogWarning ("Item Type NOT Set!");

			return;
		}

		// Check Gold

		if (inventory.GoldCount () < currShopItem.Cost ()) return;

		// Deduct Gold

		for (int i = currShopItem.Cost (); i > 0; i--)
		{
			GoldMeter.GMInstance.MinusGold ();

			// Obsolete Gold Handling. Consider removing.

			inventory.RemoveItem (ItemType.Gold);
		}

		// Make Sale

		currShopItem.Disable ();
		currShopItem.gameObject.SetActive (false);

		// Add to Inventory

		switch (currShopItem.Type ())
		{
			case ItemType.PotionOfHealth:
				playerHealth.GetHealed ();
				break;
			case ItemType.PotionOfStrength:
				playerAttack.SetDamage (2, true);
				break;
			default: inventory.AddItem (currShopItem.Type ());
				break;
		}

		itemIcon.FlashByItemType (currShopItem.Type ());

		// Remove Item from Shop

		shopItems.Remove (currShopItem);
	}
}
