using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundAudio : MonoBehaviour
{
	// Singleton

	private static BackgroundAudio _instance;
	public static BackgroundAudio BGMA_Instance { get { return _instance; } }

	[Header ("Components:")]
	[SerializeField]
	private AudioSource bgmSource;
	[SerializeField]
	private AudioSource ambientAudioSource;

	[SerializeField]
	private AudioClip _dungeon_Ambience;
	[SerializeField]
	private AudioClip _potionTrader_Ambience;
	[SerializeField]
	private AudioClip _smithy_Ambience;

	private void Awake ()
	{
		// Singleton

		if (_instance != null & _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);
	}

	private void Play_DungeonAmbience ()
	{
		ambientAudioSource.clip = _dungeon_Ambience;
		ambientAudioSource.volume = 0.25f;
	}

	private void Play_PotionTraderAmbience ()
	{
		ambientAudioSource.clip = _potionTrader_Ambience;
		ambientAudioSource.volume = 0.5f;
	}

	private void Play_SmithyAmbience ()
	{
		ambientAudioSource.clip = _smithy_Ambience;
		ambientAudioSource.volume = 0.5f;
	}

	public void Check_BackgroundAudio ()
	{
		switch (SceneManager.GetActiveScene ().name)
		{
			case "Potion Trader":
				Play_PotionTraderAmbience ();
				break;
			case "Smithy":
				Play_SmithyAmbience ();
				break;
			default:
				Play_DungeonAmbience ();
				break;
		}

		if (!bgmSource.isPlaying)
			bgmSource.Play ();
		if (!ambientAudioSource.isPlaying)
			ambientAudioSource.Play ();
	}

	public void Stop_BackgroundAudio ()
	{
		if (bgmSource.isPlaying)
			bgmSource.Stop ();
		if (ambientAudioSource.isPlaying)
			ambientAudioSource.Stop ();
	}
}