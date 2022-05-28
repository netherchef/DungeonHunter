using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientGuardAnimatorFunctions : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	// Attacking

	private int animHash_Attacking = Animator.StringToHash ("Attacking");
	public void Set_Attacking (bool state) { animator.SetBool (animHash_Attacking, state); }
	public bool Is_Attacking () { return animator.GetBool (animHash_Attacking); }

	// Release

	private int animHash_Release = Animator.StringToHash ("Release");
	public void Set_Release () { animator.SetTrigger (animHash_Release); }

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead (bool state) { animator.SetBool (animHash_Dead, state); }
	public bool Is_Dead () { return animator.GetBool (animHash_Dead); }

	// Walking

	private int animHash_Walking = Animator.StringToHash ("Walking");
	public void Set_Walking (bool state) { animator.SetBool (animHash_Walking, state); }
	public bool Is_Walking () { return animator.GetBool (animHash_Walking); }
}
