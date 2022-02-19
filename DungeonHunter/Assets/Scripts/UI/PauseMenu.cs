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

	// Variables

	private bool paused;

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

			//if (Input.GetButtonDown ("Cancel"))
			//{
			//	overlayCanvas.SetActive (true);

			//	Color tempCol = blackOverlay.color;
			//	tempCol.a = 0;
			//	blackOverlay.color = tempCol;

			//	while (Input.GetButton ("Cancel"))
			//	{
			//		if (blackOverlay.color.a >= 1)
			//		{
			//			Exit();
			//		}
			//		else
			//		{
			//			tempCol = blackOverlay.color;
			//			tempCol.a += Time.deltaTime;
			//			blackOverlay.color = tempCol;
			//		}

			//		yield return null;
			//	}

			//	overlayCanvas.SetActive (false);
			//}

			yield return null;
		}
	}

	//public static bool Paused ()
	//{
	//	return pause;
	//}

	public void Exit ()
	{
		Application.Quit ();
#if UNITY_EDITOR
		print("Quit!");
#endif
	}
}