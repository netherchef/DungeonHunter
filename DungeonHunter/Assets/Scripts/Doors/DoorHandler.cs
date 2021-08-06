using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour
{
	[Header ("Components:")]

	public Transform doorContainer;

	// Variables

	private Door[] doors;

	// Enumerators

	private IEnumerator checkDoors;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		Execute ();
	}

	public void Prep ()
	{
		doors = DoorsFromContainer (doorContainer);
	}

	public void Execute ()
	{
		checkDoors = CheckDoors ();
		StartCoroutine (checkDoors);
	}

	private IEnumerator CheckDoors ()
	{
		while (enabled)
		{
			foreach (Door door in doors)
			{
				if (door.triggered) SceneManager.LoadScene (door.transform.name);

				yield return null;
			}

			yield return null;
		}
	}

	private Door[] DoorsFromContainer (Transform container)
	{
		Door[] tempDoors = new Door[container.childCount];

		for (int i = 0; i < tempDoors.Length; i++)
		{
			tempDoors[i] = container.GetChild (i).GetComponent<Door> ();
		}

		return tempDoors;
	}
}