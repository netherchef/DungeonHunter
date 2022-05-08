using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorFunctions : MonoBehaviour
{
	// Components

	[SerializeField]
	private Animator animator;

	// Facing Right

	private int animHash_FacingRight = Animator.StringToHash ("FacingRight");

	public void Set_FacingRight (bool state) { animator.SetBool (animHash_FacingRight, state); }
	public bool Is_FacingRight () { return animator.GetBool (animHash_FacingRight); }

	// Wind Up

	private int animHash_WindUpDone = Animator.StringToHash ("WindUpDone");

	public void Set_WindUpDone_True () { animator.SetBool (animHash_WindUpDone, true); } // Accessed through Editor
	public void Set_WindUpDone_False() { animator.SetBool (animHash_WindUpDone, false); }
	public bool Is_WindUpDone () { return animator.GetBool (animHash_WindUpDone); }

	// Attack

	private int animHash_AttackStart = Animator.StringToHash ("AttackStart");
	private int animHash_AttackDone = Animator.StringToHash ("AttackDone");

	public void Set_AttackStart () { animator.SetTrigger (animHash_AttackStart); }
	public void Set_AttackDone () { animator.SetTrigger (animHash_AttackDone); } // Accessed through Editor
	public bool Is_AttackDone () { return animator.GetBool (animHash_AttackDone); }

	// Moving

	private int animHash_Moving = Animator.StringToHash ("Moving");

	public void Set_Moving(bool state) { animator.SetBool (animHash_Moving, state); }
	public bool Is_Moving () { return animator.GetBool (animHash_Moving); }

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");

	public void Set_Dead_True () { animator.SetBool (animHash_Dead, true); }
	public void Set_Dead_False () { animator.SetBool (animHash_Dead, false); }
	public bool Is_Dead () { return animator.GetBool (animHash_Dead); }

	// Summon

	private int animHash_SummonStart = Animator.StringToHash ("SummonStart");
	private int animHash_SummonDone = Animator.StringToHash ("SummonDone");

	public void Set_SummonStart () { animator.SetTrigger (animHash_SummonStart); }
	public void Set_SummonDone () { animator.SetTrigger (animHash_SummonDone); } // Accessed through Editor
	public bool Is_SummonDone () { return animator.GetBool (animHash_SummonDone); } 
}
