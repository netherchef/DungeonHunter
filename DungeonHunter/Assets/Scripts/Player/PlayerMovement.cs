using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header ("Components:")]

	public Transform master;

	[Header ("Scripts:")]

	public SceneBounds sceneBounds;

	[SerializeField]
	private PlayerAnimator animator;

	// Variables

	private float speed = 2f;

	public void Move (Vector2 dir)
	{
		animator.AnimateByDir (dir);

		Vector3 tempPos = master.position;

		tempPos += new Vector3 (dir.x, dir.y) * speed * Time.deltaTime;

		tempPos = sceneBounds.ClampPointInBounds (tempPos);

		master.position = tempPos;
	}
}