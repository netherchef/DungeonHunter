using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
	private void Update ()
	{
		if (Input.GetButtonDown ("Interact"))
		{
			print ("Hi");
		}
	}
}
