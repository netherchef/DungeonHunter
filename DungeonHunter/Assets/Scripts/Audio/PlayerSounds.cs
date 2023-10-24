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
	private AudioClip _coin_PickUp_Sound;
	[SerializeField]
	private AudioClip[] _footstep_Sounds;
	private int currFootstepIndex;

	[SerializeField]
	private AudioSource _deathSoundSource;

	public void Play_SwordSwing ()
	{
		audioSource.PlayOneShot (swordSwing);
	}

	public void Play_Gold_Sound ()
	{
		audioSource.PlayOneShot (_coin_PickUp_Sound);
	}

	public void Play_FootstepSound ()
	{
		audioSource.PlayOneShot (_footstep_Sounds[currFootstepIndex], 0.05f);

		if (currFootstepIndex < 1) currFootstepIndex++;
		else currFootstepIndex = 0;
	}

	public void Play_DeathSound ()
	{
		_deathSoundSource.PlayOneShot (_deathSoundSource.clip, 4f);
	}
}