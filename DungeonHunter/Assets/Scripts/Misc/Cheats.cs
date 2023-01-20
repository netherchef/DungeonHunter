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
	public bool greedIsGood;

	private void Awake ()
	{
		// Singleton

		if (_instance != null & _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);
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

		if (greedIsGood)
		{
			PlayerInventory pInvent = GameObject.Find ("Jake").GetComponent<PlayerInventory> ();

			for (int i = 10; i > 0; i--)
			{
				pInvent.AddItem (ItemType.Gold);
			}

			greedIsGood = false;
		}
	}
}
