using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
	[Header ("Components:")]

	public GameObject[] lootPrefabs;

	public Loot[] activeLoots;
	private Loot[] prefabLoots;

	[Header ("Scripts:")]

	public PlayerInventory inventory;
	public ItemIcon itemIcon;

	// Enumerators

	private IEnumerator collectLoot;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		Execute ();
	}

	public void Prep ()
	{
		activeLoots = LootsFromContainer (transform, true);

		// Initialise Prefab Loots

		List<Loot> tempPrefabLoots = new List<Loot> ();

		foreach (GameObject lootPrefab in lootPrefabs)
		{
			tempPrefabLoots.Add (lootPrefab.GetComponent<Loot> ());
		}

		prefabLoots = tempPrefabLoots.ToArray ();
	}

	public void Execute ()
	{
		collectLoot = CollectLoot ();
		StartCoroutine (collectLoot);
	}

	private IEnumerator CollectLoot ()
	{
		while (enabled)
		{
			foreach (Loot loot in activeLoots)
			{
				if (loot.collected)
				{
					loot.collected = false;
					loot.gameObject.SetActive (false);

					inventory.AddItem (loot.type);

					activeLoots = LootsFromContainer (transform, true);

					itemIcon.FlashByItemType (loot.type);
				}

				yield return null;
			}

			yield return null;
		}
	}

	public void DropGold (Vector3 dropPos)
	{
		Loot[] loots = LootsFromContainer (transform);

		for (int i = 0; i < loots.Length; i++)
		{
			if (loots[i].type == ItemType.Gold && !loots[i].gameObject.activeSelf)
			{
				loots[i].transform.position = dropPos;
				loots[i].gameObject.SetActive (true);

				activeLoots = LootsFromContainer (transform, true);

				return;
			}
		}

		Instantiate (LootPrefab (ItemType.Gold), dropPos, Quaternion.identity, transform);

		// Initialise

		activeLoots = LootsFromContainer (transform, true);
	}

	public void DropLoot (ItemType[] drops, Vector3 dropPos)
	{
		Loot[] loots = LootsFromContainer (transform);

		foreach (ItemType lootType in drops)
		{
			bool found = false;

			// Activate Existing Loot

			for (int i = 0; i < loots.Length; i++)
			{
				if (lootType == loots[i].type && !loots[i].gameObject.activeSelf)
				{
					loots[i].transform.position = dropPos;
					loots[i].gameObject.SetActive (true);

					found = true;

					i = loots.Length;
				}
			}

			// Spawn New Loot

			if (!found) Instantiate (LootPrefab (lootType), dropPos, Quaternion.identity, transform);
		}

		// Initialise

		activeLoots = LootsFromContainer (transform, true);
	}

	private GameObject LootPrefab (ItemType type)
	{
		foreach (Loot l in prefabLoots)
		{
			if (l.type == type) return l.gameObject;
		}

		return null;
	}

	private Loot[] LootsFromContainer (Transform container, bool activeOnly = false)
	{
		List<Loot> loots = new List<Loot> ();

		foreach (Transform child in container)
		{
			if (activeOnly)
			{
				if (child.gameObject.activeSelf) loots.Add (child.GetComponent<Loot> ());
			}
			else
			{
				loots.Add (child.GetComponent<Loot> ());
			}
		}

		return loots.ToArray ();
	}
}
