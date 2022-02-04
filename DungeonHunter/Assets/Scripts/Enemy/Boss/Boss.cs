using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType { NULL, GreatBeholder, SkeleMech }

public class Boss : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform trans;
	[SerializeField]
	private Transform pTrans;

	[SerializeField]
	private Transform pupilContainer;
	[SerializeField]
	private Transform pupil;

	[Header ("Scripts:")]

	[SerializeField]
	private HealthSystem bossHealth;
	[SerializeField]
	private HealthSystem pHealth;

	[SerializeField]
	private DoorHandler doorHandler;

	[Header ("Boss Scripts:")]

	[SerializeField]
	private GreatBeholder greatBeholder;
	[SerializeField]
	private SkeleMech skeleMech;

	[Header ("Variables:")]

	[SerializeField]
	private BossType type;

	// Enumerators

	private IEnumerator bossLoop;

	// !!! TEMPORARY !!!
	private void Start ()
	{
		Prep ();
		Execute ();
	}

	public void Prep ()
	{
		switch (type)
		{
			case BossType.GreatBeholder:
				bossLoop = GBLoop ();
				break;
		}
	}

	public void Execute () { StartCoroutine (bossLoop); }

	private IEnumerator GBLoop ()
	{
		// Intro

		// Main Loop

		while (!bossHealth.Dead () && !pHealth.Dead ())
		{
			// Phase 1

			yield return greatBeholder.Misc (trans, pTrans, pupil, pupilContainer); // Misc Action
			//yield return greatBeholder.Attack (trans, pTrans, bossHealth); // Attack

			// Phase 2
		}

		// Death

		if (pHealth.Dead ()) { } // Player Death
		if (bossHealth.Dead ()) yield return greatBeholder.Death (); // Boss Death

		// Unlock Doors

		doorHandler.Unlock ();
	}
}