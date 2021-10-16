using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpArch : MonoBehaviour
{
	// Components

	public Transform target;

	// Variables

	private float distance;
	private Vector3 corePos;

	[SerializeField]
	private bool jumping;

	//[SerializeField]
	//private bool resetPos;
	//[SerializeField]
	//private bool move;
	[SerializeField]
	private Vector3 startPos;
	[SerializeField]
	private Vector3 endPos;
	[SerializeField]
	private float moveSpeed = 1f;
	[SerializeField]
	private float archHeight = 2f;

	// Debug

	[SerializeField]
	private bool testJump;

	//private void Start ()
	//{
	//	startPos = Vector3.zero;

	//	corePos = target.position;
	//}

	private void Update ()
	{
		if (jumping)
		{
			if (Vector3.Distance (corePos, endPos) > 0.2f)
			{
				// Move Core

				Vector3 currPos = corePos;

				float speed = moveSpeed;

				if (Vector3.Distance (startPos, corePos) / distance > 0.5f) speed *= 1.5f;

				currPos += Vector3.Normalize (endPos - currPos) * speed * Time.deltaTime;
				corePos = currPos;

				// Calculate Jump Arch

				distance = Vector3.Distance (startPos, endPos);
				float amount = (Vector3.Distance (startPos, corePos) / distance) * 3.142f;

				Vector3 tempPos = corePos;

				// Apply Jump Arch modifier

				tempPos.y += Mathf.Sin (amount) * archHeight;

				// Apply the new position to the transform

				target.position = tempPos;
			}
			else
			{
				jumping = false;
				target.position = endPos;

#if UNITY_EDITOR
				if (testJump) testJump = false;
#endif
			}

			return;
		}

#if UNITY_EDITOR
		if (testJump) Jump (startPos, endPos, moveSpeed, archHeight);
#endif

		//if (move)
		//{
		//	Vector3 currPos = corePos;

		//	if (Vector3.Distance (currPos, endPos) > 0.1f)
		//	{
		//		currPos += Vector3.Normalize (endPos - currPos) * moveSpeed * Time.deltaTime;
		//	}

		//	corePos = currPos;
		//}

		//if (resetPos)
		//{
		//	resetPos = false;
		//	corePos = Vector3.zero;
		//}

		//if (Vector3.Distance (corePos, endPos) > 0)
		//{
		//	// Calculate Jump Arch depending on the core's current position
		//	// in relation with the distance between the Start and End positions.

		//	distance = Vector3.Distance (startPos, endPos);
		//	float amount = (Vector3.Distance (startPos, corePos) / distance) * 3.142f;

		//	Vector3 tempPos = corePos;

		//	// Apply Jump Arch modifier

		//	tempPos.y += Mathf.Sin (amount) * archHeight;

		//	// Apply the new position to the transform

		//	target.position = tempPos;
		//}
	}

	public void Jump (Vector3 start, Vector3 end, float speed = 4f, float height = 2f)
	{
		if (!jumping)
		{
			startPos = start;
			endPos = end;
			moveSpeed = speed;
			archHeight = height;

			corePos = start;

			jumping = true;

			return;
		}

//#if UNITY_EDITOR
//		Debug.LogWarning ("Already Jumping!");
//#endif
	}

	public bool Is_Jumping () { return jumping; }
}
