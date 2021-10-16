using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorDirection { NULL, Up, Down, Left, Right }

public class Door : MonoBehaviour
{
	//[Header ("Scripts:")]

	//[SerializeField]
	//private PlayerInputHandler inputHandler;

	[Header ("Variables:")]

	public DoorDirection direction;
	public bool triggered;

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.CompareTag ("Player"))
		{
			triggered = true;

//			switch (direction)
//			{
//				case DoorDirection.Up:
//					if (PlayerInputHandler.Direction ().y > 0 && direction == DoorDirection.Up) triggered = true;
//					break;

//				case DoorDirection.Down:
//					if (PlayerInputHandler.Direction ().y < 0 && direction == DoorDirection.Down) triggered = true;
//					break;

//				case DoorDirection.Left:
//					if (PlayerInputHandler.Direction ().x < 0 && direction == DoorDirection.Left) triggered = true;
//					break;

//				case DoorDirection.Right:
//					if (PlayerInputHandler.Direction ().x > 0 && direction == DoorDirection.Right) triggered = true;
//					break;

//				case DoorDirection.NULL:
//#if UNITY_EDITOR
//					Debug.LogWarning ("Direction NOT set for door: " + transform.name);
//#endif
			//		break;
			//}
		}
	}
}
