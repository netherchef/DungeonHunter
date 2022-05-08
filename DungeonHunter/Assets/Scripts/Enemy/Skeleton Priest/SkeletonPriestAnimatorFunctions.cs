using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriestAnimatorFunctions : MonoBehaviour
{
	// Components

    private Animator Anim { get { return GetComponent<Animator> (); } }

	// Summon Start & Done

	private int animHash_SummonStart = Animator.StringToHash ("SummonStart");
	public void Set_SummonStart () { Anim.SetTrigger (animHash_SummonStart); }

	private int animHash_SummonDone = Animator.StringToHash ("SummonDone");
	public void Set_SummonDone () { Anim.SetTrigger (animHash_SummonDone); }

	// Panic

	private int animHash_Panic = Animator.StringToHash ("Panic");
	public void Set_Panic (bool state) { Anim.SetBool (animHash_Panic, state); }
	public bool Is_Panic () { return Anim.GetBool (animHash_Panic); }

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead () { Anim.SetTrigger (animHash_Dead); }

	// Facing Right

	private int animHash_FacingRight = Animator.StringToHash ("FacingRight");
	public void Set_FacingRight(bool state) { Anim.SetBool (animHash_FacingRight, state); }
	public bool Is_FacingRight() { return Anim.GetBool (animHash_FacingRight); }

	// PrepPray

	private int animHash_PrepPray = Animator.StringToHash ("PrepPray");
	public void Set_PrepPray() { Anim.SetTrigger (animHash_PrepPray); }
	public bool Is_PrepPray() { return Anim.GetBool (animHash_PrepPray); }

	// StartPray

	private int animHash_StartPray = Animator.StringToHash ("StartPray");
	public void Set_StartPray() { Anim.SetTrigger (animHash_StartPray); } // Accessed Externally
	public bool Is_StartPray() { return Anim.GetBool (animHash_StartPray); }

	// DonePray

	private int animHash_DonePray = Animator.StringToHash ("DonePray");
	public void Set_DonePray() { Anim.SetTrigger (animHash_DonePray); }
	public bool Is_DonePray() { return Anim.GetBool (animHash_DonePray); }
}