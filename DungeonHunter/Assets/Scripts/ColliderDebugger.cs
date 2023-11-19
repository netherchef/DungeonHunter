using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDebugger : MonoBehaviour
{
	[SerializeField]
	private Transform _master;
	[SerializeField]
	private CircleCollider2D _col;
	[SerializeField]
	private Bat _batScript;

	private void OnDrawGizmos ()
	{
		if (!_col.enabled) return;

		switch (_batScript.Is_Attacking ())
		{
			case true:
				Gizmos.color = Color.red;
				break;
			case false:
				Gizmos.color = Color.blue;
				break;
		}

		Gizmos.DrawWireSphere (_master.position, _col.radius);
	}
}
