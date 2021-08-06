using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
	public Transform bodyHolder;
	public Transform body;

	public BatCollider batCollider;

	// Variables

	private Vector3 direction;
	private float speed = 2f;

	private float freqX = 4f;
	private float ampX = 1f;
	private float freqY = 32f;
	private float ampY = .2f;

	public void SetDirection (Vector3 playerPos)
	{
		direction = Vector3.Normalize (playerPos - bodyHolder.position);
	}

	public void Move ()
	{
		bodyHolder.Translate (direction * speed * Time.deltaTime);
	}

	public void OscillateBody ()
	{
		Vector3 tempPos = body.position;
		tempPos = bodyHolder.position + new Vector3 (Mathf.Cos (Time.time * freqX) * ampX, Mathf.Cos (Time.time * freqY) * ampY);
		body.position = tempPos;
	}
}
