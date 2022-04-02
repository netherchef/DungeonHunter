using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Shot
{
	public string shotName;
	public Sprite sprite;
	public string text;

	public AudioClip audioclip;
	//public float audioDuration;
	public float audioVolume;
}

public class CutSceneHandler : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Image sceneImage;
	[SerializeField]
	private Text sceneText;
	[SerializeField]
	private List<AudioSource> audioSources = new List<AudioSource> ();

	[Header ("Variables:")]

	[SerializeField]
	private Shot[] shots;

	[Header ("Skip:")]

	[SerializeField]
	private string nextScene;

	[SerializeField]
	private GameObject skipButton;

	// Debug

	//public bool proceed;

	private void Start ()
	{
		StartCoroutine(PlayCutScene(shots));
		StartCoroutine ("ShowSkipBtn");
	}

	//private void Start () { StartCoroutine (MainLoop ()); }

	//private IEnumerator MainLoop ()
	//{
	//	while (enabled)
	//	{
	//		while (!proceed) yield return null;
	//		proceed = false;

	//		yield return PlayCutScene (shots);
	//	}
	//}

	private IEnumerator ShowSkipBtn ()
	{
		while (enabled)
		{
			// Skip Button

			if (Input.GetButtonDown ("Cancel"))
			{
				if (!skipButton.activeSelf) skipButton.SetActive (true);
			}

			yield return null;
		}
	}

	private IEnumerator PlayCutScene (Shot[] shots)
	{
		if (sceneImage.color.a > 0) sceneImage.color = new Color (1, 1, 1, 0); // Hide Image
		if (sceneText.color.a > 0) sceneText.color = new Color (1, 1, 1, 0); // Hide Text

		foreach (Shot shot in shots)
		{
			yield return ChangeShot (shot);

			while (!Input.GetButtonDown ("Interact")) yield return null;

			//while (!proceed) yield return null;
			//proceed = false;
		}

		yield return Hide (sceneImage, sceneText); // Hide Image & Text

		SceneManager.LoadScene ("Entrance"); // Load Level
	}

	private IEnumerator ChangeShot (Shot shot)
	{
		// Hide Image & Text

		yield return Hide (sceneImage, sceneText);

		// Change Image

		sceneImage.sprite = shot.sprite;

		// Change Text

		sceneText.text = shot.text;

		// Change Audio

		//if (shot.sound != null) sceneAudio = shot.sound;

		// Show Image & Text

		yield return Show (sceneImage, sceneText, shot);

		// Reset Audio

		//sceneAudio = null;
	}

	private IEnumerator Show(Image image, Text text, Shot shot)
	{
		// Sound

		if (shot.audioclip != null)
		{
			//StartCoroutine (PlaySceneSound (shot));

			foreach (AudioSource audioSrc in audioSources)
			{
				if (audioSrc.clip == null)
				{
					audioSrc.clip = shot.audioclip;
					audioSrc.volume = shot.audioVolume;
					audioSrc.Play ();

					break;
				}
			}

			AudioSource newSource = gameObject.AddComponent<AudioSource> ();
			newSource.clip = shot.audioclip;
			newSource.volume = shot.audioVolume;
			audioSources.Add (newSource);

			newSource.Play ();
		}

		// Show Image

		while (image.color.a < 1)
		{
			Color tempColor = image.color;
			tempColor.a += Time.deltaTime;

			image.color = tempColor;

			yield return null;
		}

		// Show Text

		while (text.color.a < 1)
		{
			Color tempColor = text.color;
			tempColor.a += Time.deltaTime;

			text.color = tempColor;

			yield return null;
		}
	}

	private IEnumerator Hide (Image image, Text text)
	{
		// Hide Image

		//while (image.color.a > 0)
		//{
		//	Color tempColor = image.color;
		//	tempColor.a -= Time.deltaTime;

		//	image.color = tempColor;

		//	yield return null;
		//}

		// Hide Text

		//while (text.color.a > 0)
		//{
		//	Color tempColor = text.color;
		//	tempColor.a -= Time.deltaTime;

		//	text.color = tempColor;

		//	yield return null;
		//}

		while (image.color.a > 0 || text.color.a > 0)
		{
			// Image

			Color tempColor = image.color;
			tempColor.a -= Time.deltaTime;

			image.color = tempColor;

			// Text

			tempColor = text.color;
			tempColor.a -= Time.deltaTime * 0.8f;

			text.color = tempColor;

			yield return null;
		}

		// Stop Sound

		foreach (AudioSource audioSource in audioSources)
		{
			if (audioSource.isPlaying) audioSource.Stop ();
		}
	}

	private IEnumerator PlaySceneSound(Shot shot)
	{
		//#if UNITY_EDITOR
		//		if (shot.audioDuration <= 0) Debug.LogWarning ("Shot Audio Duration is 0");
		//		if (shot.audioVolume <= 0) Debug.LogWarning ("Shot Audio Volume is 0");
		//#endif

		//		foreach (AudioSource source in audioSources)
		//		{
		//			if (source.clip = null)
		//			{
		//				print ("Reusing Audio Source!");

		//				source.clip = shot.audioclip;
		//				source.volume = shot.audioVolume;
		//				source.Play ();

		//				for (float soundDur = shot.audioDuration; soundDur > 0; soundDur -= Time.deltaTime)
		//				{
		//					yield return null;
		//				}

		//				source.Stop ();

		//				source.clip = null;

		//				yield break;
		//			}

		//			yield return null;
		//	}

		//	AudioSource newSource = gameObject.AddComponent<AudioSource> ();
		//	audioSources.Add(newSource);

		//		newSource.clip = shot.audioclip;
		//		newSource.volume = shot.audioVolume;
		//		newSource.Play();

		//		for (float soundDur = shot.audioDuration; soundDur > 0; soundDur -= Time.deltaTime)
		//		{
		//			yield return null;
		//		}

		//		newSource.Stop ();

		//		newSource.clip = null;

		yield return null;
	}

	//private void Set_SceneImage (Shot shot)
	//{
	//	sceneImage.sprite = shot.sprite;
	//	sceneText.text = shot.text;
	//}

	private Shot GetShot (string shotName)
	{
		foreach (Shot shot in shots)
		{
			if (shot.shotName == shotName) return shot;
		}

#if UNITY_EDITOR
		Debug.LogWarning ("Shot: " + shotName + " NOT found!");
#endif

		return default;
	}

	public void SkipToNextScene ()
	{
		SceneManager.LoadScene (nextScene);
	}
}