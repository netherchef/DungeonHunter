using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform playerTransform;

	[Header ("Scripts:")]
	[SerializeField]
	private SceneBounds sceneBounds;
	[SerializeField]
	private PlayerDirection direction;

	[Header ("Variables:")]

	[SerializeField]
	private float distance = 1.5f;
	[SerializeField]
	private float dodgeSpeed = 6f;

	public IEnumerator DoDodge ()
	{
		Vector3 dir = direction.GetDirection ();

		Vector3 dest = playerTransform.position + dir * distance;

		bool dodging = true;

		while (dodging)
		{
			Vector3 next = Vector3.Lerp (playerTransform.position, dest, dodgeSpeed * Time.deltaTime);

			if (sceneBounds.WithinBounds (next))
			{
				playerTransform.position = next;

				dodging = Vector3.Magnitude (playerTransform.position - dest) > 0.2f;
			}
			else
			{
				dodging = false;
			}

			yield return null;
		}
	}
}
