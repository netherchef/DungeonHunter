﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
	[Header ("Scripts:")]

	[SerializeField]
	private PlayerAnimator animFuncs;

	public PlayerAttack playerAttack;
	public PlayerMovement playerMovement;
	[SerializeField]
	private PlayerDirection direction;
	[SerializeField]
	private Dodge dodge;
	[SerializeField]
	private HealthSystem healthSystem;

	//public SpriteFlipper spriteFlipper;

	public Shop shop;

	// Variables

	[SerializeField]
	private bool _becomeGod;

	// Enumerators

	private IEnumerator doInput;

	// !!! TEMPORARY !!!
	public void Execute ()
	{
		doInput = DoInput ();
		StartCoroutine (doInput);
	}

	private void Update ()
	{
		if (_becomeGod)
		{
			_becomeGod = false;
			DataPasser.DPInstance.Toggle_Godmode ();
		}
	}

	private IEnumerator DoInput ()
	{
		while (!healthSystem.Is_Dead ())
		{
			// Dodge

			if (Input.GetButtonDown ("Dodge")) yield return dodge.DoDodge (healthSystem);

			// Submit

			if (Input.GetButtonDown ("Interact") || Input.GetMouseButtonDown (0))
			{
				if (shop) shop.MakePurchase (); // Shop

				//else yield return playerAttack.Attack (direction.GetDirection ()); // Attack
				yield return playerAttack.Attack (direction.GetDirection ()); // Attack
			}

			// Direction

			Vector2 dirInput = new Vector2 (0, 0);
			
			if (InputKeyHandler.IKH_Instance.Right_Active ()) dirInput.x = 1;
			else if (InputKeyHandler.IKH_Instance.Left_Active ()) dirInput.x = -1;

			if (InputKeyHandler.IKH_Instance.Up_Active ()) dirInput.y = 1;
			else if (InputKeyHandler.IKH_Instance.Down_Active ()) dirInput.y = -1;

			// Record Direction Changes

			if (dirInput != new Vector2 (0, 0))
			{
				direction.SetDirection (Vector3.Normalize (dirInput));

				// Move in Direction

				if (!animFuncs.Is_Moving ()) animFuncs.Set_Moving (true);

				playerMovement.Move (Vector3.Normalize (dirInput));
			}
			else
			{
				if (animFuncs.Is_Moving ()) animFuncs.Set_Moving (false);
			}

			yield return null;
		}
	}
}
