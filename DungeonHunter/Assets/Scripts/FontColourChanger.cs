using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontColourChanger : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	private float _timeProg;

	private void Update ()
	{
		_timeProg += Time.deltaTime;

		Color32 tempCol = _text.faceColor;

		float alphaVal = Mathf.Sin (_timeProg);
		
		if (alphaVal < 0)
		{
			_timeProg = 0;
			alphaVal = 0;

			tempCol.a = 0;
			_text.faceColor = tempCol;

			return;
		}

		tempCol.a = (byte) (alphaVal * 255f);
		
		_text.faceColor = tempCol;
	}
}
