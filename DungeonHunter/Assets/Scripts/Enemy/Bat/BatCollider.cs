using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCollider : MonoBehaviour
{
	[SerializeField]
	private Bat _batScript;

	public bool triggered;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
		{
			if (!_batScript.Is_Attacking ()) return;

			triggered = true;
		}
	}
}
