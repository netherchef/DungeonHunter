using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
	[Header ("Scripts:")]

	public PlayerAttack playerAttack;
	public PlayerMovement playerMovement;
	public PlayerDirection direction;
	public Dodge dodge;

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
			// Dodge

			if (Input.GetButtonDown ("Dodge")) yield return dodge.DoDodge ();

			// Submit

			if (Input.GetButtonDown ("Submit"))
			{
				// Shop

				if (shop) shop.MakePurchase ();

				// Attack

				else yield return playerAttack.Attack ();
			}

			// Direction

			Vector2 dirInput = new Vector2 (0, 0);

			if (Input.GetAxisRaw ("Horizontal") > 0)
			{
				if (!facingRight)
				{
					facingRight = !facingRight;
					spriteFlipper.Flip ();
				}

				dirInput.x = 1;
			}
			else if (Input.GetAxisRaw ("Horizontal") < 0)
			{
				if (facingRight)
				{
					facingRight = !facingRight;
					spriteFlipper.Flip ();
				}

				dirInput.x = -1;
			}

			if (Input.GetAxisRaw ("Vertical") > 0)
			{
				dirInput.y = 1;
			}
			else if (Input.GetAxisRaw ("Vertical") < 0)
			{
				dirInput.y = -1;
			}

			// Record Direction Changes

			if (dirInput != new Vector2 (0, 0)) direction.SetDirection (Vector3.Normalize (dirInput));

			// Move in Direction

			playerMovement.Move (Vector3.Normalize (dirInput));

			yield return null;
		}
	}
}
