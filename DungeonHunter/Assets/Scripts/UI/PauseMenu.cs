using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	// Static Instance

	private static PauseMenu _instance;
	public static PauseMenu PMInstance { get { return _instance; } }

	[Header ("Components:")]

	[SerializeField]
	private GameObject overlayCanvas;
	[SerializeField]
	private Image blackOverlay;

	// Scripts

	private JSONeer JSONeer { get { return GetComponent<JSONeer> (); } }

	// Variables

	private bool paused;

	private bool release;

	private IEnumerator DoPauseMenuSeq { get { return DoPauseMenu (); } }

	private void Awake ()
	{
		// Static Instance

		if (_instance != null && _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);

		// Pause Menu

		StartCoroutine (DoPauseMenuSeq);
	}

	private IEnumerator DoPauseMenu ()
	{
		while (enabled)
		{
			// Only enable after Released

			while (!release) yield return null;

			if (Input.GetButtonDown ("Cancel"))
			{
				if (!paused)
				{
					paused = true;

					Time.timeScale = 0f;

					overlayCanvas.SetActive(true);
				}
				else
				{
					paused = false;

					Time.timeScale = 1f;

					overlayCanvas.SetActive (false);
				}
			}

			yield return null;
		}
	}

	//public static bool Paused ()
	//{
	//	return pause;
	//}

	public void Save () { JSONeer.Save (SceneManager.GetActiveScene ().name); }

	public void Exit ()
	{
		Application.Quit ();
#if UNITY_EDITOR
		print("Quit!");
#endif
	}

	public bool Is_Released ()
	{
		return release;
	}

	public void Release ()
	{
		release = true;
	}
}