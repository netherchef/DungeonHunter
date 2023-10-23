using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
	private Button _button { get { return GetComponent<Button> (); } }

	// Components

	[SerializeField]
	private AudioSource _audioSource;
	[SerializeField]
	private AudioClip _hoverSound;
	[SerializeField]
	private AudioClip _clickSound;

	[SerializeField]
	private float _volume = 1f;
	[SerializeField]
	private float _pitch = 1f;

	public void Play_HoverSound ()
	{
		_audioSource.pitch = _pitch;
		_audioSource.PlayOneShot (_hoverSound, _volume);
	}

	public void Play_ClickSound ()
	{
		_audioSource.pitch = _pitch;
		_audioSource.PlayOneShot (_clickSound, _volume);
	}
}
