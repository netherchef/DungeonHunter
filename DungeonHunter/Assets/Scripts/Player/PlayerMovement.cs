using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header ("Components:")]

	public Transform master;

	[Header ("Scripts:")]

	public SceneBounds sceneBounds;

	// Variables

	private float speed = 2f;

	public void Move (Vector2 dir)
	{
		Vector3 tempPos = master.position;

		//float denom = Mathf.Abs (dir.x) + Mathf.Abs (dir.y);

		//if (Mathf.Abs(denom) < Mathf.Epsilon) return;

		//dir /= denom;

		tempPos += new Vector3 (dir.x, dir.y) * speed * Time.deltaTime;

		tempPos = sceneBounds.ClampPointInBounds (tempPos);

		master.position = tempPos;
	}
}