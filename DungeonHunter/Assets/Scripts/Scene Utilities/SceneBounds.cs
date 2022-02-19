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

	[Header ("Floor:")]

	private const float floorOffset = -0.1875f;

	[Header ("Debug:")]

	public bool debug;

	//private void Start ()
	//{
	//	if (_instance != null && _instance != this) Destroy (this);
	//	else _instance = this;
	//}

#if UNITY_EDITOR

	//!!! TEMPORARY !!!
	private void OnEnable()
	{
		PrepWalls (
			GameObject.Find("Wall Top").transform,
			GameObject.Find("Wall Bottom").transform,
			GameObject.Find("Wall Left").transform,
			GameObject.Find("Wall Right").transform
			);

		PositionCorners (
			GameObject.Find("Wall_Corner TL").transform,
			GameObject.Find("Wall_Corner TR").transform,
			GameObject.Find("Wall_Corner BL").transform,
			GameObject.Find("Wall_Corner BR").transform
			);

		PrepFloor (GameObject.Find("Floor").transform);
	}

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

	#region Walls ______________________________________________________________________

	private void PrepWalls (Transform topWall, Transform btmWall, Transform leftWall, Transform rightWall)
	{
		Scale_HoriWall (topWall);
		Position_TopWall (topWall);

		Scale_HoriWall (btmWall);
		Position_BtmWall (btmWall);

		Scale_VertWall (leftWall);
		Position_LeftWall (leftWall);

		Scale_VertWall (rightWall);
		Position_RightWall (rightWall);
	}

	private void Scale_HoriWall(Transform wall)
	{
		wall.localScale = new Vector3(width * 2f, wall.localScale.y, wall.localScale.z);
	}

	private void Scale_VertWall (Transform wall)
	{
		wall.localScale = new Vector3(wall.localScale.x, height * 2f + (1f / 32f * 24f), wall.localScale.z);
	}

	// Top

	private void Position_TopWall (Transform topWall)
	{
		topWall.localPosition = new Vector3 (topWall.localPosition.x, height / 2f, topWall.localPosition.z);
	}

	// Btm

	private void Position_BtmWall (Transform btmWall)
	{
		btmWall.localPosition = new Vector3 (btmWall.localPosition.x, -(height / 2f + (1f / 32f * 12f)), btmWall.localPosition.z);
	}

	// Left

	private void Position_LeftWall (Transform leftWall)
	{
		leftWall.localPosition = new Vector3 (-width / 2f, floorOffset, leftWall.localPosition.z);
	}

	// Right

	private void Position_RightWall (Transform rightWall)
	{
		rightWall.localPosition = new Vector3 (width / 2f, floorOffset, rightWall.localPosition.z);
	}

	#endregion

	#region Floor ______________________________________________________________________

	private void PrepFloor (Transform floor)
	{
		Scale_Floor (floor);
		Position_Floor (floor);
	}

	private void Scale_Floor (Transform floor)
	{
		floor.localScale = new Vector3 (width, height + 1f / 32f * 12f, floor.localScale.z);
	}

	private void Position_Floor (Transform floor)
	{
		floor.localPosition = new Vector3 (floor.localPosition.x, floorOffset, floor.localPosition.z);
	}

	#endregion

	#region Corners ____________________________________________________________

	private void PositionCorners (Transform topLeft, Transform topRight, Transform btmLeft, Transform btmRight)
	{
		topLeft.localPosition = TopLeft ();
		topRight.localPosition = TopRight ();
		btmLeft.localPosition = BtmLeft () - new Vector3 (0, 1f / 32f * 12f);
		btmRight.localPosition = BtmRight () - new Vector3(0, 1f / 32f * 12f);
	}

	#endregion

#endif

	public bool WithinBounds(Vector3 point)
	{
		if (point.x < -width / 2 || point.x > width / 2) return false;
		if (point.y < -height / 2 || point.y > height / 2) return false;

		return true;
	}

	public Vector3 ClampPointInBounds(Vector3 point)
	{
		point.x = Mathf.Clamp(point.x, -width / 2, width / 2);
		point.y = Mathf.Clamp(point.y, -height / 2, height / 2);

		return point;
	}

	public Vector3 RandomPointInBounds()
	{
		return new Vector3(Random.Range(-width / 2, width / 2), Random.Range(height / 2, -height / 2));
	}

	public Vector3 TopRight() { return new Vector3(width / 2, height / 2, 0); }
	public Vector3 BtmRight() { return new Vector3(width / 2, -height / 2, 0); }
	public Vector3 BtmLeft() { return new Vector3(-width / 2, -height / 2, 0); }
	public Vector3 TopLeft() { return new Vector3(-width / 2, height / 2, 0); }
}