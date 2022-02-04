using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class SceneSelector : MonoBehaviour
{
	[SerializeField]
	private List<string> sceneNames;

	[SerializeField]
	private bool refresh;

#if UNITY_EDITOR
	private void Update ()
	{
		if (refresh)
		{
			refresh = false;

			sceneNames = new List<string> ();

			foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
			{
				if (s.path.Contains ("Entrance") || s.path.Contains ("Boss"))
				{

				}
				else
				{
					sceneNames.Add (Path.GetFileNameWithoutExtension (s.path));
				}
			}
		}
	}
#endif
}