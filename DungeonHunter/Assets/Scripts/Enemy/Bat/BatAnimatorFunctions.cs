using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimatorFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private GameObject master;

	[SerializeField]
	private Animator animator;

	// Dead

	private int animHash_Dead = Animator.StringToHash ("Dead");
	public void Set_Dead (bool state) { animator.SetBool (animHash_Dead, state); }

	public void Disable () { master.SetActive (false); } // Accessed through Editor
}