using System.Collections;
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

	[Header("Walls:")]

	[SerializeField]
	private Transform topWall;
	[SerializeField]
	private Transform btmWall;
	[SerializeField]
	private Transform leftWall;
	[SerializeField]
	private Transform rightWall;

	[Header ("Debug:")]

	public bool debug;

	//private void Start ()
	//{
	//	if (_instance != null && _instance != this) Destroy (this);
	//	else _instance = this;
	//}

	//!!! TEMPORARY !!!
	private void Start()
	{
		Prep ();
	}

	public void Prep ()
	{
		PrepWalls ();
	}

#if UNITY_EDITOR
	private void Update ()
	{
		if (!debug) return;

		Debug.DrawLine (new Vector2 (-width / 2, height / 2), new Vector2 (width / 2, height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (width / 2, height / 2), new Vector2 (width / 2, -height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (width / 2, -height / 2), new Vector2 (-width / 2, -height / 2), Color.magenta);
		Debug.DrawLine (new Vector2 (-width / 2, -height / 2), new Vector2 (-width / 2, height / 2), Color.magenta);
	}

	private void OnDrawGizmos ()
	{
		if (!debug) return;

		// Visualize Corners

		Gizmos.DrawSphere (new Vector3 (width / 2, height / 2, 0), 0.2f); // Top Right
		Gizmos.DrawSphere (new Vector3 (width / 2, -height / 2, 0), 0.2f); // Btm Right
		Gizmos.DrawSphere (new Vector3 (-width / 2, -height / 2, 0), 0.2f); // Btm Left
		Gizmos.DrawSphere (new Vector3 (-width / 2, height / 2, 0), 0.2f); // Top Left
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

	public Vector3 TopRight () { return new Vector3 (width / 2, height / 2, 0); }
	public Vector3 BtmRight () { return new Vector3 (width / 2, -height / 2, 0); }
	public Vector3 BtmLeft () { return new Vector3 (-width / 2, -height / 2, 0); }
	public Vector3 TopLeft () { return new Vector3 (-width / 2, height / 2, 0); }

	#region Walls ______________________________________________________________________

	private void PrepWalls ()
	{
		Scale_TopWall ();
		Position_TopWall ();

		Scale_BtmWall ();
		Position_BtmWall ();

		Scale_LeftWall ();
		Position_LeftWall ();

		Scale_RightWall ();
		Position_RightWall ();
	}

	// Top

	private void Scale_TopWall ()
	{
		topWall.localScale = new Vector3 (width * 2f, topWall.localScale.y, topWall.localScale.z);
	}

	private void Position_TopWall ()
	{
		topWall.localPosition = new Vector3 (topWall.localPosition.x, height / 2f, topWall.localPosition.z);
	}

	// Btm

	private void Scale_BtmWall ()
	{
		btmWall.localScale = new Vector3 (width * 2f, btmWall.localScale.y, btmWall.localScale.z);
	}

	private void Position_BtmWall ()
	{
		btmWall.localPosition = new Vector3 (btmWall.localPosition.x, -(height / 2f + (1f / 32f * 12f)), btmWall.localPosition.z);
	}

	// Left

	private void Scale_LeftWall ()
	{
		leftWall.localScale = new Vector3 (leftWall.localScale.x, height * 2f + (1f / 32f * 24f), leftWall.localScale.z);
	}

	private void Position_LeftWall ()
	{
		leftWall.localPosition = new Vector3 (-width / 2f, leftWall.localPosition.y, leftWall.localPosition.z);
	}

	// Right

	private void Scale_RightWall ()
	{
		rightWall.localScale = new Vector3 (rightWall.localScale.x, height * 2f + (1f / 32f * 24f), rightWall.localScale.z);
	}

	private void Position_RightWall ()
	{
		rightWall.localPosition = new Vector3 (width / 2f, rightWall.localPosition.y, rightWall.localPosition.z);
	}

	#endregion
}