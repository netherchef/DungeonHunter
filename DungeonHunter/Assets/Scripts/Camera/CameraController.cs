using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CamDirection { Up, Down, Left, Right }

public class CameraController : MonoBehaviour
{
	// Components

	[SerializeField]
	private Transform trans;

	// Variables

	private Vector3 startPos;
	[SerializeField]
	private float stepSize = 1f;
	[SerializeField]
	private int stepsFromEnd = 5;
	[SerializeField]
	private int easingSteps = 5;
	[SerializeField]
	private float interval = 0.05f;

	public CamDirection direction;
	private bool active;

	public void Execute ()
	{
		if (startPos == Vector3.zero) startPos = trans.position;

		switch (direction)
		{
			case CamDirection.Up:
				StartCoroutine (MoveCamY (stepSize, stepsFromEnd, easingSteps, 1));
				break;
			case CamDirection.Down:
				StartCoroutine (MoveCamY (stepSize, stepsFromEnd, easingSteps, -1));
				break;
			case CamDirection.Left:
				StartCoroutine (MoveCamX (stepSize, stepsFromEnd, easingSteps, -1));
				break;
			case CamDirection.Right:
				StartCoroutine (MoveCamX (stepSize, stepsFromEnd, easingSteps, 1));
				break;
		}
	}

	private IEnumerator MoveCamY (float size, int steps, int eases, int dir)
	{
		active = true;

		trans.position = startPos + new Vector3 (0, size * steps * -dir, 0);

		while (steps > 1)
		{
			steps--;

			trans.position += new Vector3 (0, size * dir, 0);

			for (float i = interval; i > 0; i -= Time.deltaTime) yield return null;

			yield return null;
		}

		while (eases > 0)
		{
			eases--;

			size /= 2f;

			trans.position += new Vector3 (0, size * dir, 0);

			for (float i = interval; i > 0; i -= Time.deltaTime) yield return null;

			yield return null;
		}

		trans.position += new Vector3 (0, size * dir, 0);

		active = false;
	}

	private IEnumerator MoveCamX (float size, int steps, int eases, int dir)
	{
		active = true;

		trans.position = startPos + new Vector3 (size * steps * -dir, 0, 0);

		while (steps > 1)
		{
			steps--;

			trans.position += new Vector3 (size * dir, 0, 0);

			for (float i = interval; i > 0; i -= Time.deltaTime) yield return null;

			yield return null;
		}

		while (eases > 0)
		{
			eases--;

			size /= 2f;

			trans.position += new Vector3 (size * dir, 0, 0);

			for (float i = interval; i > 0; i -= Time.deltaTime) yield return null;

			yield return null;
		}

		trans.position += new Vector3 (size * dir, 0, 0);

		active = false;
	}

	public bool IsActive () { return active; }
}
