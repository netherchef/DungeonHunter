using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
	public GameObject deathScreen;

	public string startingScene = "Entrance";

	// Enumerators

	private IEnumerator doDeath;

	public void ShowDeathScreen ()
	{
		deathScreen.SetActive (true);

		doDeath = DoDeath ();
		StartCoroutine (doDeath);
	}

	private IEnumerator DoDeath ()
	{
		if (Input.GetButtonDown ("Submit")) yield return null;

		while (!Input.GetButtonDown ("Submit")) yield return null;

		// !!! TEMPORARY !!!
		DataPasser.DPInstance.playerCurrHp = 4;

		SceneManager.LoadScene (startingScene);

		yield return null;
	}
}
