using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smithy : MonoBehaviour
{
	private ArmorShopItem[] armorShopItems;

	private void Start()
	{
		ArmorShopItem[] temp = new ArmorShopItem[transform.childCount];

		for (int i = 0; i < temp.Length; i++)
		{
			temp[i] = transform.GetChild (i).GetComponent<ArmorShopItem> ();
		}

		armorShopItems = temp;

	}

	private void Update ()
	{
		if (Input.GetKeyDown ("t"))
		{
			print (DataPasser.DPInstance.CurrentArmorType ());
		}

		//if (Input.GetButtonDown ("Interact"))
		//{
		//	foreach (ArmorShopItem shopItem in armorShopItems)
		//	{
		//		if (shopItem.PlayerDetected ())
		//		{
		//			if (DataPasser.DPInstance.CurrentArmorType () != shopItem.ShopArmorType ())
		//			{
		//				BuyArmor (shopItem.ShopArmorType ());
		//				shopItem.gameObject.SetActive (false);
		//				return;
		//			}
		//		}
		//	}
		//}
	}

	//private void BuyArmor (ArmorType type)
	//{
	//	DataPasser.DPInstance.SetArmorType (type);
	//}
}