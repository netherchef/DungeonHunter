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

	//private InputMaster inputMaster;

	// Variables

	//private static bool pause;

	private bool quitting;

	private IEnumerator DoPauseMenuSeq { get { return DoPauseMenu (); } }

	private void Awake ()
	{
		// Static Instance

		if (_instance != null && _instance != this) Destroy (gameObject);
		else _instance = this;

		DontDestroyOnLoad (this);

		// Input

		//inputMaster = new InputMaster ();
		//inputMaster.Player.SetCallbacks (this);

		//inputMaster.Player.Enable ();

		// Pause Menu

		StartCoroutine (DoPauseMenuSeq);
	}

	//public void OnInteract (InputAction.CallbackContext context) { }
	//public void OnDodge (InputAction.CallbackContext context) { }
	//public void OnDirection (InputAction.CallbackContext context) { }

	//public void OnPause (InputAction.CallbackContext context)
	//{
	//	//pause = !pause;

	//	if (context.started) quitting = true;
	//	else if (context.canceled) quitting = false;
	//}

	private IEnumerator DoPauseMenu ()
	{
		while (enabled)
		{
			if (Input.GetButtonDown ("Cancel"))
			{
				overlayCanvas.SetActive (true);

				Color tempCol = blackOverlay.color;
				tempCol.a = 0;
				blackOverlay.color = tempCol;

				while (Input.GetButton ("Cancel"))
				{
					if (blackOverlay.color.a >= 1)
					{
						Application.Quit (); // Quit

#if UNITY_EDITOR
						print ("Quit!");
#endif
					}
					else
					{
						tempCol = blackOverlay.color;
						tempCol.a += Time.deltaTime;
						blackOverlay.color = tempCol;
					}

					yield return null;
				}

				overlayCanvas.SetActive (false);
			}

			//if (pause)
			//{
			//	Time.timeScale = 0;

			//	while (pause) yield return null;

			//	Time.timeScale = 1;
			//}

			yield return null;
		}
	}

	//public static bool Paused ()
	//{
	//	return pause;
	//}
}
