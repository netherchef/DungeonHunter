using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
	public Transform master;

	public void Flip ()
	{
		Vector3 tempScale = master.localScale;
		tempScale.x *= -1;
		master.localScale = tempScale;
	}
}