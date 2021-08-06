using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
	public List<LootType> loots;

	public void AddLoot (LootType lootType)
	{
		loots.Add (lootType);
	}
}
