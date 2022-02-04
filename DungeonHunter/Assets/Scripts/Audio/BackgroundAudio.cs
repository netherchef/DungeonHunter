using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
	// Singleton

	private static BackgroundAudio _instance;

	[Header ("Components:")]
	[SerializeField]
	private AudioSource bgmSource;
	[SerializeField]
	private AudioSource ambientAudioSource;

	private void Awake ()
	{
		// Singleton

		if (_instance != null & _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);
	}
}