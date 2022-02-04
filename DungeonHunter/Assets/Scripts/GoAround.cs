using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAround : MonoBehaviour
{
	public Transform master;
	public Transform target;

	[SerializeField]
	private bool chase;

	public float moveSpeed = 2f;
	public float turnSpeed = 72f;
	public float range = 2f;

	//private Vector3 moveDir = new Vector3 ();

	//private bool sideStepping = false;

	private int obstacleLayer;

	// Enumerators

	private IEnumerator goToTarget;

	private void Start ()
	{
		obstacleLayer = LayerMask.GetMask ("Obstacle");
	}

	private void Update ()
	{
		if (chase)
		{
			chase = false;

			if (goToTarget == null)
			{
				goToTarget = GoToTarget (master, target);

				StartCoroutine (goToTarget);
			}
		}
	}

	//private void Update ()
	//{
	//	if (Input.GetButtonDown ("Submit")) chase = !chase;

	//	if (!chase) return;

	//	Debug.DrawLine (master.position, master.position + moveDir * range);

	//	master.position += moveDir * moveSpeed * Time.deltaTime;
	//}

	//private void FixedUpdate ()
	//{
	//	// Direction to Target

	//	Vector3 dirToTarg = Vector3.Normalize (target.position - master.position);

	//	Debug.DrawLine (master.position, master.position + dirToTarg * range, Color.magenta);

	//	// Check for Obstacle

	//	RaycastHit2D obstacle = Physics2D.Raycast (master.position, dirToTarg, range, obstacleLayer);
	//	if (obstacle)
	//	{
	//		// If there is an Obstacle between us and the target, 
	//		// and we're NOT Sidestepping:

	//		if (!sideStepping)
	//		{
	//			sideStepping = true;

	//			moveDir = dirToTarg;

	//			// Rotate Vector Until No Obstacle

	//			int leftScore = 20;
	//			int rightScore = 20;

	//			bool checking = true;

	//			Vector3 checkDirLeft = dirToTarg;

	//			while (rightScore > 0 && checking)
	//			{
	//				rightScore--;

	//				Debug.DrawLine (master.position, master.position + checkDirLeft * (range * 2f), Color.green);

	//				checkDirLeft = RotateVector (checkDirLeft, turnSpeed);

	//				checking = Physics2D.Raycast (master.position, checkDirLeft, range * 4f, obstacleLayer);
	//			}

	//			checking = true;

	//			Vector3 checkDirRight = dirToTarg;

	//			while (leftScore > 0 && checking)
	//			{
	//				leftScore--;

	//				Debug.DrawLine (master.position, master.position + checkDirRight * (range * 2f), Color.red);

	//				checkDirRight = RotateVector (checkDirRight, -turnSpeed);

	//				checking = Physics2D.Raycast (master.position, checkDirRight, range * 4f, obstacleLayer);
	//			}

	//			Debug.Break ();

	//			print ("Left: " + leftScore + " | Right: " + rightScore);

	//			if (rightScore > leftScore)
	//			{
	//				// Go Right

	//				moveDir = checkDirRight;

	//				print ("Going Right");
	//			}
	//			else
	//			{
	//				// Go Left

	//				moveDir = checkDirLeft;

	//				print ("Going Left");
	//			}
	//		}

	//		// If there is an Obstacle but we are already Sidestepping,
	//		// just keep moving in the same direction.
	//	}
	//	else
	//	{
	//		moveDir = dirToTarg;

	//		if (sideStepping) sideStepping = false;
	//	}
	//}

	private IEnumerator GoToTarget (Transform chaser, Transform targ)
	{
		Vector3 dest = targ.position; // Decide on Destination

		// Check if there is an Obstacle between the Chaser and the Destination

		Vector3 dirToDest = Vector3.Normalize (dest - chaser.position);

		// If there is an Obstacle ---------------------------------------------

		//if (Physics2D.Raycast (chaser.position, dirToDest, range, obstacleLayer))
		if (Blocked (chaser.position, dest))
		{
			// Scan Left

			Vector3 scanDirLeft = dirToDest;

			int leftSteps = 0;

			for (leftSteps = 0; leftSteps < 10; leftSteps++)
			{
				if (!Physics2D.Raycast (chaser.position, scanDirLeft, range, obstacleLayer)) break;
				else scanDirLeft = RotateVector (scanDirLeft, turnSpeed);

#if UNITY_EDITOR
				Debug.DrawLine (chaser.position, chaser.position + (scanDirLeft * range), Color.green);
#endif
			}

			// Scan Right

			Vector3 scanDirRight = dirToDest;

			int rightSteps = 0;

			for (rightSteps = 0; rightSteps < 10; rightSteps++)
			{
				if (!Physics2D.Raycast (chaser.position, scanDirRight, range, obstacleLayer)) break;
				else scanDirRight = RotateVector (scanDirRight, -turnSpeed);

#if UNITY_EDITOR
				Debug.DrawLine (chaser.position, chaser.position + (scanDirRight * range), Color.red);
#endif
			}

			// Set the Move Direction to the direction with the least number of steps
			print ("Left: " + leftSteps + " | Right: " + rightSteps);
			Vector3 movementDir = leftSteps < rightSteps ? scanDirLeft : scanDirRight;

			// Move in the Move Direction until the line of sight between
			// the Chaser and the Destination is NOT blocked.

			//bool blocked = true;

			while (Blocked (chaser.position, dest))
			{
				chaser.position += movementDir * moveSpeed * Time.deltaTime; // Move

				// Check for Blockage

				dirToDest = Vector3.Normalize (dest - chaser.position);

				//blocked = Physics2D.Raycast (chaser.position, dirToDest, range, obstacleLayer);

				yield return null;
			}

			// Now that there's nothing blocking, Move to the Destination.

			while (Vector3.Distance (chaser.position, dest) > 0.1f)
			{
				chaser.position += dirToDest * moveSpeed * Time.deltaTime;

				yield return null;
			}

		} // If there is NO Obstacle -------------------------------------------
		else
		{
			// Move to the Destination

			while (Vector3.Distance (chaser.position, dest) > 0.1f)
			{
				chaser.position += dirToDest * moveSpeed * Time.deltaTime;

				yield return null;
			}
		}

		// !!! Reset Enumerator !!!

		goToTarget = null;
	}

	private IEnumerator DoGoAround (Transform chaser, Transform targ)
	{
		yield return null;
	}

	Vector3 RotateVector (Vector3 vector, float degree)
	{
		return Quaternion.Euler (0, 0, degree) * vector;
	}

	public bool Blocked (Vector3 origin, Vector3 targetPos)
	{
		return Physics2D.Raycast (origin, Vector3.Normalize (targetPos - origin), range, obstacleLayer);
	}

	// Bunny83
	// https://answers.unity.com/questions/1229302/rotate-a-vector2-around-the-z-axis-on-a-mathematic.html
}
