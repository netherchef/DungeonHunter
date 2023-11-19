using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform playerTransform;

	// Audio

	[SerializeField]
	private AudioSource _dodgeAudioSource;

	[Header ("Scripts:")]
	[SerializeField]
	private SceneBounds sceneBounds;
	[SerializeField]
	private PlayerDirection direction;

	[Header ("Variables:")]

	[SerializeField]
	private float distance = 1.5f;
	[SerializeField]
	private float dodgeSpeed = 10f;
	[SerializeField]
	private bool _dodging;

	public IEnumerator DoDodge (HealthSystem health)
	{
		// Set Direction

		Vector3 dir = direction.GetDirection ();

		// Set Destination

		Vector3 dest = playerTransform.position + dir * distance;

		// Dodge Sound

		_dodgeAudioSource.PlayOneShot (_dodgeAudioSource.clip);

		_dodging = true;

		while (_dodging && !health.Is_Dead ())
		{
			Vector3 next = Vector3.Lerp (playerTransform.position, dest, dodgeSpeed * Time.deltaTime);

			if (sceneBounds.WithinBounds (next))
			{
				playerTransform.position = next;

				// Check if Still Dodging

				_dodging = Vector3.Magnitude (playerTransform.position - dest) > 0.2f;
			}
			else
			{
				_dodging = false;
			}

			yield return null;
		}
	}

	public bool Is_Dodging () { return _dodging; }
}
