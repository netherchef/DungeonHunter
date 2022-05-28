using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatBeholderAnimatorFunctions : MonoBehaviour
{
    // Components

    private Animator Anim { get { return GetComponent<Animator> (); } }

    // Attacking

    private int animHash_Attacking = Animator.StringToHash ("Attacking");
    public void Set_Attacking (bool state) { Anim.SetBool (animHash_Attacking, state); }
    public bool Is_Attacking () { return Anim.GetBool (animHash_Attacking); }

    // Hurt

    private int animHash_Hurt = Animator.StringToHash ("Hurt");
    public void Set_Hurt (bool state) { Anim.SetBool (animHash_Hurt, state); }
    public bool Is_Hurt () { return Anim.GetBool (animHash_Hurt); }

    // Dead

    private int animHash_Dead = Animator.StringToHash ("Dead");
    public void Set_Dead (bool state) { Anim.SetBool (animHash_Dead, state); }
    public bool Is_Dead () { return Anim.GetBool (animHash_Dead); }
    public void Set_Dead_True () { Anim.SetBool (animHash_Dead, true); } // Accessed Externally
}
