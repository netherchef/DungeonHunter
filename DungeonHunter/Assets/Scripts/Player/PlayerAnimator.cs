using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	// Moving

	private int animHash_Moving = Animator.StringToHash ("Moving");
	private void Set_Moving (bool state) { animator.SetBool (animHash_Moving, state); }
	public bool Is_Moving () { return animator.GetBool (animHash_Moving); }

	// Facing Up, Down, Left, Right

	private int animHash_FacingUp = Animator.StringToHash ("FacingUp");
	private void Set_FacingUp () { animator.SetTrigger (animHash_FacingUp); }
	public bool Is_FacingUp () { return animator.GetBool (animHash_FacingUp); }

	private int animHash_FacingDown = Animator.StringToHash ("FacingDown");
	private void Set_FacingDown () { animator.SetTrigger (animHash_FacingDown); }
	public bool Is_FacingDown () { return animator.GetBool (animHash_FacingDown); }

	private int animHash_FacingLeft = Animator.StringToHash ("FacingLeft");
	private void Set_FacingLeft () { animator.SetTrigger (animHash_FacingLeft); }
	public bool Is_FacingLeft () { return animator.GetBool (animHash_FacingLeft); }

	private int animHash_FacingRight = Animator.StringToHash ("FacingRight");
	private void Set_FacingRight () { animator.SetTrigger (animHash_FacingRight); }
	public bool Is_FacingRight () { return animator.GetBool (animHash_FacingRight); }

	public void AnimateByDir (Vector2 dir)
	{
		if (dir.x > 0)
		{
			Set_FacingRight ();
			return;
		}
		else if (dir.x < 0)
		{
			Set_FacingLeft ();
			return;
		}

		if (dir.y > 0) Set_FacingUp ();
		else if (dir.y < 0) Set_FacingDown ();
	}
}
