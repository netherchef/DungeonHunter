using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
	[Header ("Scripts:")]

	public PlayerAttack playerAttack;

	public PlayerMovement playerMovement;

	public SpriteFlipper spriteFlipper;

	public Shop shop;

	[Header ("Variables:")]

	private bool facingRight;

	// Enumerators

	private IEnumerator doInput;

	private void Start ()
	{
		doInput = DoInput ();
		StartCoroutine (doInput);
	}

	private IEnumerator DoInput ()
	{
		while (enabled)
		{
			// Submit

			if (Input.GetButtonDown ("Submit"))
			{
				// Shop

				if (shop) shop.MakePurchase ();

				// Attack

				else yield return playerAttack.Attack ();
			}

			// Direction

			Vector2 playerInput = new Vector2 (0, 0);

			if (Input.GetAxisRaw ("Horizontal") > 0)
			{
				if (!facingRight)
				{
					facingRight = !facingRight;
					spriteFlipper.Flip ();
				}

				playerInput.x = 1;
			}
			else if (Input.GetAxisRaw ("Horizontal") < 0)
			{
				if (facingRight)
				{
					facingRight = !facingRight;
					spriteFlipper.Flip ();
				}

				playerInput.x = -1;
			}

			if (Input.GetAxisRaw ("Vertical") > 0)
			{
				playerInput.y = 1;
			}
			else if (Input.GetAxisRaw ("Vertical") < 0)
			{
				playerInput.y = -1;
			}

			playerMovement.Move (playerInput);

			yield return null;
		}
	}
}
