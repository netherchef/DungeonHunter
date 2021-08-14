using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SceneBounds : MonoBehaviour
{
	[Header ("Dimensions:")]

	public int width = 10;
	public int height = 10;

	[Header ("Debug:")]

	public bool debug;

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
}