using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneOrbShot : MonoBehaviour
{
	private Vector2 initPos;
	private float speed = 2f;

	private void OnEnable()
	{
		initPos = transform.position;
	}

	public void Shoot (Vector2 dir)
	{
		StartCoroutine (DoShoot (dir));
	}

    private IEnumerator DoShoot (Vector2 dir)
	{
		while (Vector2.Distance (initPos, transform.position) < 5f)
		{
			transform.Translate (dir * speed * Time.deltaTime);

			yield return null;
		}

		gameObject.SetActive (false);
	}
}
