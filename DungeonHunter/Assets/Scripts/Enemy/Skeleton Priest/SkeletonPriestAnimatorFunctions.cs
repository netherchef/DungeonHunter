using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriestAnimatorFunctions : MonoBehaviour
{
	// Components

    private Animator Anim { get { return GetComponent<Animator> (); } }

	private int animHash_PrepPray = Animator.StringToHash ("PrepPray");
	private int animHash_StartPray = Animator.StringToHash ("StartPray");
	private int animHash_DonePray = Animator.StringToHash ("DonePray");

	public void Set_PrepPray () { Anim.SetTrigger (animHash_PrepPray); }
	public bool Is_PrepPray () { return Anim.GetBool (animHash_PrepPray); }

	public void Set_StartPray () { Anim.SetTrigger (animHash_StartPray); } // Accessed Externally
	public bool Is_StartPray () { return Anim.GetBool (animHash_StartPray); }

	public void Set_DonePray () { Anim.SetTrigger (animHash_DonePray); }
	public bool Is_DonePray () { return Anim.GetBool (animHash_DonePray); }
}
