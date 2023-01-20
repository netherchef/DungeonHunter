using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicBars : MonoBehaviour
{
	[SerializeField]
	private GameObject barCanvas;
	[SerializeField]
	private RectTransform barTop;
	private float posTop = -10f;
	[SerializeField]
	private RectTransform barBottom;
	private float posBtm = 10f;

	// Variables

	private float speed = 64f;
		
	public bool start;
	[SerializeField]
	private bool end;

	// Coroutines

	private IEnumerator cineBars;

	//private void Update ()
	//{
	//	if (start)
	//	{
	//		start = false;
	//		ShowCinematicBars ();
	//	}
	//}

	private IEnumerator CineBars ()
	{
		// Top

		Vector3 tempPos = barTop.anchoredPosition;
		tempPos.y = posTop + 100f;
		barTop.anchoredPosition = tempPos;

		// Bottom

		tempPos = barBottom.anchoredPosition;
		tempPos.y = posBtm - 100f;
		barBottom.anchoredPosition = tempPos;

		// Show

		barCanvas.SetActive (true);

		while (barTop.anchoredPosition.y > posTop || barBottom.anchoredPosition.y < posBtm)
		{
			if (barTop.anchoredPosition.y > posTop)
			{
				tempPos = barTop.anchoredPosition;
				tempPos.y -= speed * Time.deltaTime;

				if (tempPos.y <= posTop)
				{
					tempPos.y = posTop;
				}

				barTop.anchoredPosition = tempPos;
			}

			if (barBottom.anchoredPosition.y < posBtm)
			{
				tempPos = barBottom.anchoredPosition;
				tempPos.y += speed * Time.deltaTime;

				if (tempPos.y >= posBtm)
				{
					tempPos.y = posBtm;
				}

				barBottom.anchoredPosition = tempPos;
			}

			yield return null;
		}

		// Wait for End

		while (!end)
		{
			yield return null;
		}

		// Hide

		while (barTop.anchoredPosition.y < posTop + 100f || barBottom.anchoredPosition.y > posBtm - 100f)
		{
			if (barTop.anchoredPosition.y < posTop + 100f)
			{
				tempPos = barTop.anchoredPosition;
				tempPos.y += speed * Time.deltaTime;

				if (tempPos.y >= posTop + 100f)
				{
					tempPos.y = posTop + 100f;
				}

				barTop.anchoredPosition = tempPos;
			}

			if (barBottom.anchoredPosition.y > posBtm - 100f)
			{
				tempPos = barBottom.anchoredPosition;
				tempPos.y -= speed * Time.deltaTime;

				if (tempPos.y <= posBtm - 100f)
				{
					tempPos.y = posBtm - 100f;
				}

				barBottom.anchoredPosition = tempPos;
			}

			yield return null;
		}

		barCanvas.SetActive (false);

		// Reset

		end = false;

		cineBars = null;
	}

	public void ShowCinematicBars ()
	{
		if (cineBars != null) return;

		cineBars = CineBars ();
		StartCoroutine (cineBars);
	}

	public void End ()
	{
		end = true;
	}
}
