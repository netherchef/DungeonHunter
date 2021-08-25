using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
	private Vector3 Direction { get; set; }

	public void SetDirection (Vector3 dir) { Direction = dir; }

	public Vector3 GetDirection () { return Direction; }
}
