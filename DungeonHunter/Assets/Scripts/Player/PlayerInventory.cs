using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	// Components

	private GoldMeter goldMeter;

	// Variables

	[SerializeField]
	private List<ItemType> items = new List<ItemType> ();

	public void AssignGoldMeter ()
	{
		goldMeter = GoldMeter.GMInstance;
	}

	public void AddItem (ItemType itemType)
	{
		//items.Add (itemType);

		if (itemType == ItemType.Gold) goldMeter.AddGold (); // Gold Meter
	}

	public void AddItems (ItemType[] itemList)
	{
		foreach (ItemType item in itemList) items.Add (item);
	}

	//public int GoldCount ()
	//{
	//	int count = 0;

	//	foreach (ItemType item in items)
	//	{
	//		if (item == ItemType.Gold) count++;
	//	}

	//	return count;
	//}

	public void RemoveItem (ItemType item)
	{
		items.Remove (item);
	}

	public ItemType[] ItemsAsArray ()
	{
		return items.ToArray ();
	}

	public bool HasItems ()
	{
		return items.Count > 0;
	}
}