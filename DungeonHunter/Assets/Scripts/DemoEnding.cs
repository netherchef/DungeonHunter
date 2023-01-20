using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnding : MonoBehaviour
{
	[Header ("Componenets:")]

	[SerializeField]
	private Camera endingCamera;
	[SerializeField]
	private GameObject endingBGCanvas;
	[SerializeField]
	private GameObject endingMessageCanvas;
	[SerializeField]
	private GameObject downIndicator;

	public IEnumerator ShowEndingMessage ()
	{
		// Show Message

		endingCamera.enabled = true;
		endingBGCanvas.SetActive (true);
		endingMessageCanvas.SetActive (true);

		// Wait 2 Seconds

		for (float timer = 2f; timer > 0; timer -= Time.deltaTime) yield return null;

		// Wait for Input

		downIndicator.SetActive (true);

		while (!Input.GetButtonDown ("Interact")) yield return null;

		downIndicator.SetActive (false);

		// Hide Message

		endingCamera.enabled = false;
		endingBGCanvas.SetActive (false);
		endingMessageCanvas.SetActive (false);
	}
}