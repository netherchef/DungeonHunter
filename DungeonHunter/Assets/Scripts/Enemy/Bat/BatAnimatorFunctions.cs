using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimatorFunctions : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead () { animator.SetTrigger (animHash_Dead); }

	public void Disable () { gameObject.SetActive (false); } // Accessed through Editor
}
