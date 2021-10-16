﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, InputMaster.IPlayerActions
{
	[Header ("Scripts:")]

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

	[Header ("Variables:")]

	private InputMaster inputMaster;

	private static Vector2 dirInput;
	private static bool interact;
	private bool attack;
	private bool doDodge;

	// Enumerators

	private IEnumerator doInput;

	private void Awake ()
	{
		inputMaster = new InputMaster ();
		inputMaster.Player.SetCallbacks (this);

		inputMaster.Player.Enable ();
	}

	public void OnInteract (InputAction.CallbackContext context)
	{
		if (context.started)
		{
			attack = true;
			interact = true;
		}
	}

	public void OnDodge (InputAction.CallbackContext context)
	{
		if (context.started)
		{
			doDodge = true;
		}
	}

	public void OnDirection (InputAction.CallbackContext context)
	{
		dirInput = context.ReadValue<Vector2> ();
	}

	// !!! TEMPORARY !!!
	private void Start ()
	{
		doInput = DoInput ();
		StartCoroutine (doInput);
	}

	private IEnumerator DoInput ()
	{
		while (!healthSystem.Dead ())
		{
			// Dodge

			if (doDodge)
			{
				doDodge = false;

				yield return dodge.DoDodge (healthSystem);
			}

			// Submit

			if (interact)
			{
				interact = false;

				if (shop) shop.MakePurchase (); // Shop
			}
			// Attack

			if (attack)
			{
				//yield return playerAttack.Attack (Vector3.Normalize (dirInput));
				yield return playerAttack.Attack (direction.GetDirection ());

				attack = false;
			}
			//}

			// Direction

			//Vector2 dirInput = new Vector2 (0, 0);

			//if (Input.GetAxisRaw ("Horizontal") > 0)
			//{
			//	//if (!facingRight)
			//	//{
			//	//	facingRight = !facingRight;
			//	//	spriteFlipper.Flip ();
			//	//}

			//	dirInput.x = 1;
			//}
			//else if (Input.GetAxisRaw ("Horizontal") < 0)
			//{
			//	//if (facingRight)
			//	//{
			//	//	facingRight = !facingRight;
			//	//	spriteFlipper.Flip ();
			//	//}

			//	dirInput.x = -1;
			//}

			//if (Input.GetAxisRaw ("Vertical") > 0) dirInput.y = 1;
			//else if (Input.GetAxisRaw ("Vertical") < 0) dirInput.y = -1;

			// Record Direction Changes

			if (dirInput != new Vector2 (0, 0)) direction.SetDirection (Vector3.Normalize (dirInput));

			// Move in Direction

			playerMovement.Move (Vector3.Normalize (dirInput));

			yield return null;
		}
	}

	public static Vector2 Direction () { return dirInput; }

	public static bool Interact_ButtonDown ()
	{
		if (interact)
		{
			interact = false;
			return true;
		}

		return false;
	}
}
