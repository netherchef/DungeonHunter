using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	[Header ("Components:")]

	[SerializeField]
	private Transform camTrans;

	[Header ("Variables:")]

	[SerializeField]
	private float power = 1f;
	[SerializeField]
	private float fallOffSpeed = 1f;

	// Enumerators

	private IEnumerator shake;

#if UNITY_EDITOR

	[Header ("Debug:")]

	[SerializeField]
	private bool doShake;
	[SerializeField]
	private bool shakeX;
	[SerializeField]
	private bool shakeY;

	private void Update ()
	{
		if (doShake)
		{
			doShake = false;

			if (shake != null) StopCoroutine (shake);

			shake = DoShake (power, fallOffSpeed);

			StartCoroutine (shake);
		}
		else if (shakeX)
		{
			shakeX = false;

			if (shake != null) StopCoroutine (shake);

			shake = ShakeX (power, fallOffSpeed);

			StartCoroutine (shake);
		}
		else if (shakeY)
		{
			shakeY = false;

			if (shake != null) StopCoroutine (shake);

			shake = ShakeY (power, fallOffSpeed);

			StartCoroutine (shake);
		}
	}
#endif

	public void Shake (float pow = 1f, float fallOff = 1f)
	{
		if (shake != null) StopCoroutine (shake);

		shake = DoShake (pow, fallOff);
		StartCoroutine (shake);
	}

	public IEnumerator DoShake (float pow = 1f, float fallOff = 1f)
	{
		float currPower = pow;

		while (currPower > 0)
		{
			Vector3 shakePos = Random.insideUnitSphere;

			camTrans.localPosition = new Vector3 (shakePos.x * currPower, shakePos.y * currPower, camTrans.localPosition.z);

			currPower -= fallOff * Time.deltaTime;

			yield return null;
		}

		camTrans.localPosition = new Vector3 (0, 0, camTrans.localPosition.z);
	}

	public IEnumerator ShakeX (float pow = 1f, float fallOff = 1f)
	{
		float currPower = pow;

		while (currPower > 0)
		{
			Vector3 shakePos = Random.insideUnitSphere;

			camTrans.localPosition = new Vector3 (shakePos.x * currPower, 0, camTrans.localPosition.z);

			currPower -= fallOff * Time.deltaTime;

			yield return null;
		}

		camTrans.localPosition = new Vector3 (0, 0, camTrans.localPosition.z);
	}

	public IEnumerator ShakeY (float pow = 1f, float fallOff = 1f)
	{
		float currPower = pow;

		while (currPower > 0)
		{
			Vector3 shakePos = Random.insideUnitSphere;

			camTrans.localPosition = new Vector3 (0, shakePos.y * currPower, camTrans.localPosition.z);

			currPower -= fallOff * Time.deltaTime;

			yield return null;
		}

		camTrans.localPosition = new Vector3 (0, 0, camTrans.localPosition.z);
	}
}