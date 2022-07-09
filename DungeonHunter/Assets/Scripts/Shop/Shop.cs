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

	[SerializeField]
	private Transform shopItemContainer;

	[SerializeField]
	private AudioSource audioSource;

	[Header ("Scripts:")]

	public HealthSystem playerHealth;
	public PlayerAttack playerAttack;
	private PlayerAnimator playerAnimator;
	public PlayerInventory inventory;
	public ItemIcon itemIcon;

	// Variables

	[SerializeField]
	private List<ShopItem> shopItems = new List<ShopItem> ();

	// !!! TEMPORARY !!!
	//private void Start () { Prep (); }

	public void Prep (PlayerAnimator pAnim)
	{
		foreach (Transform child in shopItemContainer)
		{
			shopItems.Add (child.GetComponent<ShopItem> ());
		}

		// Player Animator for Switching Armor Animation

		playerAnimator = pAnim;
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

		if (currShopItem == null) return; // If No Shop Item was Found, Cancel Purchase

		if (currShopItem.Type () == ItemType.NULL)
		{
			Debug.LogWarning ("Item Type NOT Set!");

			return;
		}
		
		// Check Gold

		if (GoldMeter.GMInstance.CurrentGold () < currShopItem.Cost ()) return;
		//if (inventory.GoldCount () < currShopItem.Cost ()) return;

		// Deduct Gold

		for (int i = currShopItem.Cost (); i > 0; i--)
		{
			GoldMeter.GMInstance.MinusGold ();

			// Obsolete Gold Handling. Consider removing.

			//inventory.RemoveItem (ItemType.Gold);
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

			case ItemType.GoldArmor:
				DataPasser.DPInstance.SetArmorType (ArmorType.Gold);
				playerAnimator.Set_Gold ();
				break;
			case ItemType.BronzeArmor:
				DataPasser.DPInstance.SetArmorType (ArmorType.Bronze);
				playerAnimator.Set_Bronze ();
				break;
			case ItemType.RubyArmor:
				DataPasser.DPInstance.SetArmorType (ArmorType.Ruby);
				playerAnimator.Set_Ruby ();
				break;
		}

		currShopItem.PlayItemSound (audioSource);

		itemIcon.FlashByItemType (currShopItem.Type ());

		// Remove Item from Shop

		shopItems.Remove (currShopItem);
	}
}
