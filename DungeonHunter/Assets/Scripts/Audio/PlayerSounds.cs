using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip swordSwing;

    public void Play_SwordSwing ()
	{
		audioSource.PlayOneShot (swordSwing);
	}
}