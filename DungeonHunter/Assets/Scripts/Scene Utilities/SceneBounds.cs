﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneBounds : MonoBehaviour
{
	//private static SceneBounds _instance;
	//public static SceneBounds SBInstance { get { return _instance; } }

	[Header ("Dimensions:")]

	[SerializeField]
	private int width = 10;
	[SerializeField]
	private int height = 10;

	[Header ("Debug:")]

	public bool debug;

	//private void Start ()
	//{
	//	if (_instance != null && _instance != this) Destroy (this);
	//	else _instance = this;
	//}

#if UNITY_EDITOR
	private void Update ()
	{
		if (!debug) return;

		Debug.DrawLine (new Vector2 (-width / 2, height / 2), new Vector2 (width / 2, height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (width / 2, height / 2), new Vector2 (width / 2, -height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (width / 2, -height / 2), new Vector2 (-width / 2, -height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (-width / 2, -height / 2), new Vector2 (-width / 2, height / 2), Color.magenta);
	}
#endif

	public bool WithinBounds (Vector3 point)
	{
		if (point.x < -width / 2 || point.x > width / 2) return false;
		if (point.y < -height / 2 || point.y > height / 2) return false;

		return true;
	}

	public Vector3 ClampPointInBounds (Vector3 point)
	{
		point.x = Mathf.Clamp (point.x, -width / 2, width / 2);
		point.y = Mathf.Clamp (point.y, -height / 2, height / 2);

		return point;
	}

	public Vector3 RandomPointInBounds ()
	{
		return new Vector3 (Random.Range (-width / 2, width / 2), Random.Range (height / 2, -height / 2));
	}
}