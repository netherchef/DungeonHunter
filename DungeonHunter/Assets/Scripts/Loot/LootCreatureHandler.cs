using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCreatureHandler : MonoBehaviour
{
	public BoxCollider2D playerAttackCollider;

	[Header ("Scripts:")]

	public LootHandler lootHandler;

	// Variables

	public LootCreature[] creatures;

	// Enumerators

	private IEnumerator checkCreatures;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		Execute ();
	}

	public void Prep ()
	{
		creatures = LootCreaturesFromContainer (transform);
	}

	public void Execute ()
	{
		checkCreatures = CheckCreatures ();
		StartCoroutine (checkCreatures);
	}

	private IEnumerator CheckCreatures ()
	{
		while (enabled)
		{
			for (int i = 0; i < creatures.Length; i++)
			{
				if (creatures[i].health.Is_Dead ())
				{
					creatures[i].gameObject.SetActive (false);

					lootHandler.DropLoot (creatures[i].drops, creatures[i].transform.position);

					creatures = LootCreaturesFromContainer (transform);
				}
			}

			yield return null;
		}
	}

	private LootCreature[] LootCreaturesFromContainer (Transform container)
	{
		List<LootCreature> tempCreatures = new List<LootCreature> ();

		foreach (Transform child in container)
		{
			if (child.gameObject.activeSelf) tempCreatures.Add (child.GetComponent<LootCreature> ());
		}

		return tempCreatures.ToArray ();
	}
}
