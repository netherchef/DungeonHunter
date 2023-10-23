using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
	// Singleton

	private static Cheats _instance;

	public static Cheats CheatsInstance { get { return _instance; } }

	public bool jumpEntrance, jumpPotion, jumpSmith;
	//public bool greedIsGood;

	private void Awake ()
	{
		// Singleton

		if (_instance != null & _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);
	}

	private void Start ()
	{
		do_CheatLoop = CheatLoop ();
		StartCoroutine (do_CheatLoop);
	}

	private void Update ()
	{
		if (jumpEntrance)
		{
			SceneManager.LoadScene ("Entrance");

			jumpEntrance = false;

			return;
		}

		if (jumpPotion)
		{
			SceneManager.LoadScene ("Potion Trader");

			jumpPotion = false;

			return;
		}

		if (jumpSmith)
		{
			SceneManager.LoadScene ("Smithy");

			jumpSmith = false;

			return;
		}

		//if (greedIsGood)
		//{
		//	PlayerInventory pInvent = GameObject.Find ("Jake").GetComponent<PlayerInventory> ();

		//	for (int i = 10; i > 0; i--)
		//	{
		//		pInvent.AddItem (ItemType.Gold);
		//	}

		//	greedIsGood = false;
		//}
	}

	private IEnumerator do_CheatLoop;

	private IEnumerator CheatLoop ()
	{
		while (enabled)
		{
			if (Input.anyKeyDown)
			{
				string currCombo = Input.inputString;

				while (currCombo != "")
				{
					if (Input.anyKeyDown) yield return null;

					bool reset = true;

					// Wait for Input

					for (float timer = 1f; timer > 0; timer -= Time.deltaTime)
					{
						if (Input.anyKeyDown)
						{
							string input = Input.inputString;
							if (input != "") currCombo += input;

							//print (currentCheat);

							CheckCheats (currCombo);

							reset = false;

							break; // Exit current Loop
						}

						yield return null;
					}

					if (reset) // No Input, Reset
					{
						//print ("Reset");
						currCombo = "";
					}

					yield return null;
				}
			}

			yield return null;
		}
	}

	private void CheckCheats (string combo)
	{
		switch (combo)
		{
			case "whosyourdaddy":
				WhosYourDaddy ();
				break;

			case "greedisgood":
				GreedIsGood ();
				break;
		}
	}

	private void WhosYourDaddy ()
	{
		print ("WHO'S YOUR DADDY?");

		try { DataPasser.DPInstance.Toggle_Godmode (); }
		catch { Debug.LogWarning ("Failed to Toggle God Mode."); }
	}

	private void GreedIsGood ()
	{
		print ("GREED IS GOOD!");

		try
		{
			if (DataPasser.DPInstance.CurrentGold () >= 20) return;

			GoldMeter gm = GameObject.Find ("Gold Meter(Clone)").GetComponent<GoldMeter> ();

			for (int i = 20 - DataPasser.DPInstance.CurrentGold (); i > 0; i--) gm.AddGold ();
		}
		catch { Debug.LogWarning ("Failed to Add Riches."); }
	}
}
