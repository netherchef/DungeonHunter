using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorFunctions : MonoBehaviour
{
	public Animator animator;

	private int animHash_AttackStart = Animator.StringToHash ("AttackStart");
	private int animHash_AttackDone = Animator.StringToHash ("AttackDone");

	public void AttackStart ()
	{
		animator.SetTrigger (animHash_AttackStart);
	}

	public bool Is_AttackDone ()
	{
		return animator.GetBool (animHash_AttackDone);
	}

	#region Set Through Editor _________________________________________________

	public void AttackDone ()
	{
		animator.SetTrigger (animHash_AttackDone);
	}

	#endregion
}
