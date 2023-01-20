using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownIndicator : MonoBehaviour
{
	// Components

	private RectTransform rectTrans { get { return GetComponent<RectTransform> (); } }

	// Variables

	private Vector2 startPos;

	private float timer = 0f;

	private bool down;

	[SerializeField]
	private float offset = 1f;

	private void OnEnable ()
	{
		if (startPos.x == 0 || startPos.y == 0) startPos = rectTrans.anchoredPosition;

		rectTrans.anchoredPosition = startPos;

		timer = 0f;

		down = false;
	}

	private void Update ()
	{
		if (timer <= 0)
		{
			timer = 0.5f;

			if (!down)
			{
				down = true;

				rectTrans.anchoredPosition -= new Vector2 (0f, offset);
			}
			else
			{
				down = false;

				rectTrans.anchoredPosition += new Vector2 (0f, offset);
			}

			return;
		}

		timer -= Time.deltaTime;
	}
}
