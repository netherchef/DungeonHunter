using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimatorFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Animator animator;

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead (bool state) { animator.SetBool (animHash_Dead, state); }
	public bool Is_Dead () { return animator.GetBool (animHash_Dead); }

	public void Set_Dead_False () { animator.SetBool (animHash_Dead, false); } // Accessed through Editor
}
