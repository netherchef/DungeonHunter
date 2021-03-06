using System.Collections;
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

	[Header ("Variables:")]

	//private InputMaster inputMaster;

	//private static Vector2 dirInput;
	//private static bool interact;
	//private bool attack;
	//private bool doDodge;

	// Enumerators

	private IEnumerator doInput;

	private void Awake ()
	{
		// Input

		//inputMaster = new InputMaster ();
		//inputMaster.Player.SetCallbacks (this);

		//inputMaster.Player.Enable ();
	}

	//public void OnInteract (InputAction.CallbackContext context)
	//{
	//	if (context.started)
	//	{
	//		attack = true;
	//		interact = true;
	//	}
	//}

	//public void OnDodge (InputAction.CallbackContext context)
	//{
	//	if (context.started) doDodge = true;
	//}

	//public void OnDirection (InputAction.CallbackContext context)
	//{
	//	dirInput = context.ReadValue<Vector2> ();
	//}

	//public void OnPause (InputAction.CallbackContext context) { }

	// !!! TEMPORARY !!!
	private void Start ()
	{
		doInput = DoInput ();
		StartCoroutine (doInput);
	}

	private IEnumerator DoInput ()
	{
		while (!healthSystem.Is_Dead ())
		{
			// Dodge

			if (Input.GetButtonDown ("Dodge")) yield return dodge.DoDodge (healthSystem);

			//if (doDodge)
			//{
			//	doDodge = false;

			//	yield return dodge.DoDodge (healthSystem);
			//}

			// Submit

			if (Input.GetButtonDown ("Interact") || Input.GetMouseButtonDown (0))
			{
				if (shop) shop.MakePurchase (); // Shop

				//else yield return playerAttack.Attack (direction.GetDirection ()); // Attack
				yield return playerAttack.Attack (direction.GetDirection ()); // Attack
			}

			//if (interact)
			//{
			//	interact = false;

			//	if (shop) shop.MakePurchase (); // Shop
			//}

			// Attack

			//if (attack)
			//{
			//	//yield return playerAttack.Attack (Vector3.Normalize (dirInput));
			//	yield return playerAttack.Attack (direction.GetDirection ());

			//	attack = false;
			//}
			//}

			// Direction

			Vector2 dirInput = new Vector2 (0, 0);

			if (Input.GetAxisRaw ("Horizontal") > 0) dirInput.x = 1;
			else if (Input.GetAxisRaw ("Horizontal") < 0) dirInput.x = -1;

			if (Input.GetAxisRaw ("Vertical") > 0) dirInput.y = 1;
			else if (Input.GetAxisRaw ("Vertical") < 0) dirInput.y = -1;

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

	//public static Vector2 Direction () { return dirInput; }

	//public static bool Interact_ButtonDown ()
	//{
	//	if (interact)
	//	{
	//		interact = false;
	//		return true;
	//	}

	//	return false;
	//}
}
