using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorFunctions : MonoBehaviour
{
	// Components

	[SerializeField]
	private Animator animator;

	// Attack

	private int animHash_AttackStart = Animator.StringToHash ("AttackStart");
	private int animHash_AttackDone = Animator.StringToHash ("AttackDone");

	public void Set_AttackStart () { animator.SetTrigger (animHash_AttackStart); }
	public void Set_AttackDone () { animator.SetTrigger (animHash_AttackDone); } // Accessed through Editor
	public bool Is_AttackDone () { return animator.GetBool (animHash_AttackDone); }

	// Summon

	private int animHash_SummonStart = Animator.StringToHash ("SummonStart");
	private int animHash_SummonDone = Animator.StringToHash ("SummonDone");

	public void Set_SummonStart () { animator.SetTrigger (animHash_SummonStart); }
	public void Set_SummonDone () { animator.SetTrigger (animHash_SummonDone); } // Accessed through Editor
	public bool Is_SummonDone () { return animator.GetBool (animHash_SummonDone); } 
}
