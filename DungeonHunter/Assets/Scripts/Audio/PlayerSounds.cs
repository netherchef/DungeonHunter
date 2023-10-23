using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip swordSwing;
	[SerializeField]
	private AudioClip _man_Hurt_Sound;

    public void Play_SwordSwing ()
	{
		audioSource.PlayOneShot (swordSwing);
	}

	public void Play_Hurt_Sound ()
	{
		audioSource.PlayOneShot (_man_Hurt_Sound);
	}
}