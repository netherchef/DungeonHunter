using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFunctions : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform master;

	[SerializeField]
	private Transform target;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem health;

	[SerializeField]
	private SlimeAnimatorFunctions animFunctions;

	[SerializeField]
	private SceneBounds sceneBounds;

	// Variables

	private float speed = 0.5f;

	// Enumerators

	private IEnumerator SlimeSeq { get { return DoSlimeSeq (); } }

	// !!! TEMPORARY !!!
	private void Start () { Execute (); }

	public void Execute ()
	{
		StartCoroutine (SlimeSeq);
	}

	private IEnumerator DoSlimeSeq ()
	{
		while (!health.Dead ())
		{
			if (!health.Dead ())
			{
				Vector3 dir = Vector3.Normalize (target.position - master.position);

				Vector3 newPos = sceneBounds.ClampPointInBounds (master.position + (dir * speed * Time.deltaTime));

				master.position = newPos;
			}

			yield return null;
		}

		// Death

		animFunctions.Set_Dead (true); // Death Animation

		while (animFunctions.Is_Dead ()) yield return null;
	}
}