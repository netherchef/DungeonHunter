using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DoorDirection { NULL, Up, Down, Left, Right }

public class Door : MonoBehaviour
{
	[Header ("Variables:")]

	//public DoorDirection direction;
	public bool triggered;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player")) triggered = true;
	}
}
