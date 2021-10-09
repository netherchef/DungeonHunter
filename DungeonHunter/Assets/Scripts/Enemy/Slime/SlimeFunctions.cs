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

	private SceneBounds sceneBounds;

	private HealthSystem targetHealth;
	private CircleCollider2D targCol;

	private LootHandler lootHandler;

	// Variables

	private float speed = 0.5f;

	// Enumerators

	private IEnumerator SlimeSeq { get { return DoSlimeSeq (); } }

	public void Execute () { StartCoroutine (SlimeSeq); }

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

	public HealthSystem HealthSystem () { return health; }

	public void Set_TargetTransform (Transform trans)
	{
		target = trans;
	}

	public void Set_TargetHealthSystem (HealthSystem healthSys)
	{
		targetHealth = healthSys;
	}

	public void Set_TargetCollider (CircleCollider2D col)
	{
		targCol = col;
	}

	public void Set_LootHandler (LootHandler handler)
	{
		lootHandler = handler;
	}

	public void Set_SceneBounds (SceneBounds sb)
	{
		sceneBounds = sb;
	}
}