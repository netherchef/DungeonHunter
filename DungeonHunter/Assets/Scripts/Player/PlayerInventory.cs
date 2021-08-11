using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	public List<ItemType> items;

	public void AddItem (ItemType itemType)
	{
		items.Add (itemType);
	}

	public int GoldCount ()
	{
		int count = 0;

		foreach (ItemType item in items)
		{
			if (item == ItemType.Gold) count++;
		}

		return count;
	}

	public void RemoveItem (ItemType item)
	{
		items.Remove (item);
	}
}