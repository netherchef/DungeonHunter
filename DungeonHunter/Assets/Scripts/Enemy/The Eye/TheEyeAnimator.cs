using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEyeAnimator : MonoBehaviour
{
	[Header ("Components:")]

	public Animator animator;

	//[Header ("Debug:")]

	//public bool debug;
	//public bool attackStart;
	//public bool attackExit;

	//private void Update ()
	//{
	//	if (!debug) return;

	//	if (attackStart)
	//	{
	//		attackStart = false;
	//		Set_AttackStart ();
	//	}
	//	else if (attackExit)
	//	{
	//		attackExit = false;
	//		Set_AttackExiting ();
	//	}
	//}

	#region Attack _____________________________________________________________

	private int animHash_AttackStart = Animator.StringToHash ("AttackStart");
	private int animHash_AttackStartDone = Animator.StringToHash ("AttackStartDone");
	private int animHash_AttackExiting = Animator.StringToHash ("AttackExiting");
	private int animHash_AttackEnd = Animator.StringToHash ("AttackEnd");

	public void Set_AttackStart () { animator.SetTrigger (animHash_AttackStart); }
	public bool Is_AttackStart () { return animator.GetBool (animHash_AttackStart); }

	public void Set_AttackStartDone () { animator.SetTrigger (animHash_AttackStartDone); } // Accessed through Animator
	public bool Is_AttackStartDone () { return animator.GetBool (animHash_AttackStartDone); }

	public void Set_AttackExiting () { animator.SetTrigger (animHash_AttackExiting); }
	public bool Is_AttackExiting () { return animator.GetBool (animHash_AttackExiting); }

	public void Set_AttackEnd () { animator.SetTrigger (animHash_AttackEnd); } // Accessed through Animator
	public bool Is_AttackEnd () { return animator.GetBool (animHash_AttackEnd); }

	#endregion
}
