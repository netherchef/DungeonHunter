using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	// Audio
	[SerializeField]
	private PlayerSounds _playerSounds;

	// Moving

	private int animHash_Moving = Animator.StringToHash ("Moving");
	public void Set_Moving (bool state) { animator.SetBool (animHash_Moving, state); }
	public bool Is_Moving () { return animator.GetBool (animHash_Moving); }

	public void Play_Footstep_Sound ()
	{
		_playerSounds.Play_FootstepSound ();
	}

	// Attacking

	private int animHash_Attacking = Animator.StringToHash ("Attacking");
	public void Set_Attacking (bool state) { animator.SetBool (animHash_Attacking, state); }
	public bool Is_Attacking () { return animator.GetBool (animHash_Attacking); }

	// Facing Up, Down, Left, Right

	private int animHash_FacingUp = Animator.StringToHash ("FacingUp");
	public void Set_FacingUp (bool state) { animator.SetBool (animHash_FacingUp, state); }
	public bool Is_FacingUp () { return animator.GetBool (animHash_FacingUp); }

	private int animHash_FacingDown = Animator.StringToHash ("FacingDown");
	public void Set_FacingDown (bool state) { animator.SetBool (animHash_FacingDown, state); }
	public bool Is_FacingDown () { return animator.GetBool (animHash_FacingDown); }

	private int animHash_FacingLeft = Animator.StringToHash ("FacingLeft");
	public void Set_FacingLeft (bool state) { animator.SetBool (animHash_FacingLeft, state); }
	public bool Is_FacingLeft () { return animator.GetBool (animHash_FacingLeft); }

	private int animHash_FacingRight = Animator.StringToHash ("FacingRight");
	public void Set_FacingRight (bool state) { animator.SetBool (animHash_FacingRight, state); }
	public bool Is_FacingRight () { return animator.GetBool (animHash_FacingRight); }

	public void AnimateByDir (Vector2 dir)
	{
		if (dir.x > 0)
		{
			Set_FacingRight (true);
			Set_FacingLeft (false);

			Set_FacingUp (false);
			Set_FacingDown (false);

			return;
		}
		else if (dir.x < 0)
		{
			Set_FacingLeft (true);
			Set_FacingRight (false);

			Set_FacingUp (false);
			Set_FacingDown (false);

			return;
		}

		if (dir.y > 0)
		{
			Set_FacingUp (true);
			Set_FacingDown (false);

			Set_FacingLeft (false);
			Set_FacingRight (false);

		}
		else if (dir.y < 0)
		{
			Set_FacingDown (true);
			Set_FacingUp (false);

			Set_FacingLeft (false);
			Set_FacingRight (false);
		}
	}

	// Death

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead (bool state) { animator.SetBool (animHash_Dead, state); }
	public bool Is_Dead () { return animator.GetBool (animHash_Dead); }

	// Gold Armor

	private int animHash_Gold = Animator.StringToHash ("Gold");
	public void Set_Gold () { animator.SetTrigger (animHash_Gold); }
	public bool Is_Gold () { return animator.GetBool (animHash_Gold); }

	// Bronze Armor

	private int animHash_Bronze = Animator.StringToHash ("Bronze");
	public void Set_Bronze () { animator.SetTrigger (animHash_Bronze); }
	public bool Is_Bronze () { return animator.GetBool (animHash_Bronze); }

	// Ruby Armor

	private int animHash_Ruby = Animator.StringToHash ("Ruby");
	public void Set_Ruby () { animator.SetTrigger (animHash_Ruby); }
	public bool Is_Ruby () { return animator.GetBool (animHash_Ruby); }
}
