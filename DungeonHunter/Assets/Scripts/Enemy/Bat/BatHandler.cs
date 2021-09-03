using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHandler : MonoBehaviour
{
	[Header ("Components:")]

	public Transform batContainer;
	public Transform playerTransform;

	[Header ("Scripts:")]

	public SceneBounds sceneBounds;
	public HealthSystem playerHealth;

	// Variables

	//private List<Bat> bats = new List<Bat> ();

	// Enumerators

	//private IEnumerator checkBats;

	//private void Start ()
	//{
	//	Prep ();
	//	Execute ();
	//}

	//public void Prep ()
	//{
	//	foreach (Transform child in batContainer)
	//	{
	//		bats.Add (child.GetComponent<Bat> ());
	//	}
	//}

	//public void Execute ()
	//{
	//	checkBats = CheckBats ();
	//	StartCoroutine (checkBats);
	//}

	//private IEnumerator CheckBats ()
	//{
	//	while (enabled)
	//	{
	//		foreach (Bat bat in bats)
	//		{
	//			// Check HP

	//			if (bat.healthSystem.currHp > 0)
	//			{
	//				// Move

	//				bat.MoveToPlayer (playerTransform.position);

	//				// Damage

	//				if (bat.batCollider.triggered)
	//				{
	//					bat.batCollider.triggered = false;

	//					playerHealth.Damage ();
	//				}
	//			}
	//		}

	//		yield return null;
	//	}
	//}
}
