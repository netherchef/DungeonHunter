using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject deathScreen;

	[Header ("Scripts:")]

	[SerializeField]
	private PlayerAnimator animator;

	[Header ("Variables:")]

	public string startingScene = "Entrance";

	// Enumerators

	private IEnumerator doDeath;

	public void StartDeath ()
	{
		doDeath = DoDeath ();
		StartCoroutine (doDeath);
	}

	private IEnumerator DoDeath ()
	{
		animator.Set_Dead (true);

		for (float timer = 3f; timer > 0; timer -= Time.deltaTime) yield return null;

		deathScreen.SetActive (true);

		if (Input.GetButtonDown ("Interact")) yield return null;
		//if (PlayerInputHandler.Interact_ButtonDown ()) yield return null;

		while (!Input.GetButtonDown ("Interact")) yield return null;

		// !!! TEMPORARY !!!
		DataPasser.DPInstance.playerCurrHp = 4;
		DataPasser.DPInstance.SetArmorType (ArmorType.NULL);
		DataPasser.DPInstance.Set_CurrDamage (1);
		DataPasser.DPInstance.Set_Items (new ItemType[0]);
		GoldMeter.GMInstance.Reset ();

		DataPasser.DPInstance.previousRoom = "";

		SceneManager.LoadScene (startingScene);

		yield return null;
	}
}
