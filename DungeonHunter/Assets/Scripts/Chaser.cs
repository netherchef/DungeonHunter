using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
	// Components

	[SerializeField]
	private Transform target;

	// Variables

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private bool lockOn;

	[SerializeField]
	private float chaseAndForget;

	private void OnEnable ()
	{
		StartCoroutine (Do_Chase);
	}

	private IEnumerator Do_Chase { get { return Chase (); } }

	private IEnumerator Chase ()
	{
		if (chaseAndForget > 0)
		{
			yield return ChaseAndForget (chaseAndForget);
		}
		else if (lockOn) yield return LockOn ();
		else yield return StraightShot (Vector3.Normalize (target.position - transform.position));
	}

	private IEnumerator StraightShot (Vector3 targDir)
	{
		while (Vector2.Distance (transform.position, target.position) < 20f)
		{
			transform.Translate (targDir * speed * 2f * Time.deltaTime);

			yield return null;
		}

		gameObject.SetActive (false);
	}

	private IEnumerator LockOn ()
	{
		while (Vector2.Distance (transform.position, target.position) > 0)
		{
			transform.Translate (Vector3.Normalize (target.position - transform.position) * speed * Time.deltaTime);

			yield return null;
		}
	}

	private IEnumerator ChaseAndForget (float chaseDur)
	{
		for (float timer = chaseDur; timer > 0; timer -= Time.deltaTime)
		{
			if (Vector2.Distance (transform.position, target.position) > 0)
			{
				transform.Translate (Vector3.Normalize (target.position - transform.position) * speed * Time.deltaTime);
			}
			else
			{
				timer = 0f;
			}

			yield return null;
		}
	}
}
