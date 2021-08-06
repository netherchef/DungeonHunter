using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorDirection { NULL, Up, Down, Left, Right }

public class Door : MonoBehaviour
{
	public DoorDirection direction;
	public bool triggered;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
		{
			switch (direction)
			{
				case DoorDirection.Up:
					if (Input.GetAxisRaw ("Vertical") > 0) triggered = true;
					break;

				case DoorDirection.Down:
					if (Input.GetAxisRaw ("Vertical") < 0) triggered = true;
					break;

				case DoorDirection.Left:
					if (Input.GetAxisRaw ("Horizontal") < 0) triggered = true;
					break;

				case DoorDirection.Right:
					if (Input.GetAxisRaw ("Horizontal") > 0) triggered = true;
					break;

				case DoorDirection.NULL:
#if UNITY_EDITOR
					Debug.LogWarning ("Direction NOT set for door: " + transform.name);
#endif
					break;
			}
		}
	}
}
