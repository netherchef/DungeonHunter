using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleMechAnim : MonoBehaviour
{
	// Components

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private GameObject groundSlamDamager;

	// Scripts

	[SerializeField]
	private CameraShaker camShaker;

	// Variables

	private int animHash_GS_Wind = Animator.StringToHash ("GS_Wind");
	public void GS_Wind () { animator.SetTrigger (animHash_GS_Wind); }

	private int animHash_GS_Slam = Animator.StringToHash ("GS_Slam");
	public void GS_Slam () { animator.SetTrigger (animHash_GS_Slam); }

	private int animHash_GS_End = Animator.StringToHash ("GS_End");
	public void GS_End () { animator.SetTrigger (animHash_GS_End); }
	public bool Is_GS_End () { return animator.GetBool (animHash_GS_End); }

	private int animHash_Fireball_Wind = Animator.StringToHash ("Fireball_Wind");
	public void Fireball_Wind () { animator.SetTrigger (animHash_Fireball_Wind); }

	private int animHash_Fireball_End = Animator.StringToHash ("Fireball_End");
	public void Fireball_End () { animator.SetTrigger (animHash_Fireball_End); }

	private int animHash_Death = Animator.StringToHash ("Death");
	public void Death () { animator.SetTrigger (animHash_Death); }

	public void GroundSlam ()
	{
		GS_Slam ();

		groundSlamDamager.SetActive (true); ;

		camShaker.Shake (0.2f, 0.5f);
	}

	public void Disable_SlamDamager ()
	{
		groundSlamDamager.SetActive (false);
	}
}
