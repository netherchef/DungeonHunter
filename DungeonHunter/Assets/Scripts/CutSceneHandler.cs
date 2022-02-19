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
}

public class CutSceneHandler : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Image sceneImage;
	[SerializeField]
	private Text sceneText;

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

		// Show Image & Text

		yield return Show (sceneImage, sceneText);
	}

	private IEnumerator Hide (Image image, Text text)
	{
		// Hide Image

		while (image.color.a > 0)
		{
			Color tempColor = image.color;
			tempColor.a -= Time.deltaTime;

			image.color = tempColor;

			yield return null;
		}

		// Hide Text

		while (text.color.a > 0)
		{
			Color tempColor = text.color;
			tempColor.a -= Time.deltaTime;

			text.color = tempColor;

			yield return null;
		}
	}

	private IEnumerator Show (Image image, Text text)
	{
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