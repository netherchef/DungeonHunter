﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
	private void Update ()
	{
		if (Input.GetButtonDown ("Interact"))
		{
			SceneManager.LoadScene ("Opening Cut Scene");
		}
	}
}
