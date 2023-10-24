using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct MenuButton
{
	public SpriteRenderer spriteRenderer;
	public Image image;
	public UnityEvent buttonEvent;
}

public class MenuButtonHandler : MonoBehaviour
{
	[SerializeField]
	private MenuButton[] menuColumn;

	[SerializeField]
	private StartScreen _startScreen;

	// Audio

	[SerializeField]
	private ButtonSounds _buttonSounds;

	// Variables

	[SerializeField]
	private int currentIndex;

	private float selectedOpacity = 1f;
	private float deselectedOpacity = 0.5f;

	// Coroutines

	private IEnumerator Do_HandlerLoop { get { return HandlerLoop (); } }

	private IEnumerator HandlerLoop ()
	{
		// Wait for InputKeyHandler to Start

		while (InputKeyHandler.IKH_Instance == null) yield return null;

		while (enabled)
		{
			if (InputKeyHandler.IKH_Instance.Interact_Start ())
			{
				menuColumn[currentIndex].buttonEvent.Invoke ();

				// Play Click Sound

				_buttonSounds.Play_ClickSound ();

				// Start Game Seq

				_startScreen.New_Game_Seq ();

				// Hide Menu

				_startScreen.Hide_StartScreen ();
			}
			else if (InputKeyHandler.IKH_Instance.Up_Start ())
			{
				GoToButton_Above ();
			}
			else if (InputKeyHandler.IKH_Instance.Down_Start ())
			{
				GoToButton_Below ();
			}

			yield return null;
		}
	}

	private void OnEnable ()
	{
		// Select First Button

		Color col;

		try
		{
			col = menuColumn[0].spriteRenderer.color;
			col.a = selectedOpacity;
			menuColumn[0].spriteRenderer.color = col;
		}
		catch
		{
			col = menuColumn[0].image.color;
			col.a = selectedOpacity;
			menuColumn[0].image.color = col;
		}

		// Deselect Other Buttons

		for (int i = 1; i < menuColumn.Length; i++)
		{	
			try
			{
				col = menuColumn[i].spriteRenderer.color;
				col.a = deselectedOpacity;
				menuColumn[i].spriteRenderer.color = col;
			}
			catch
			{
				col = menuColumn[i].image.color;
				col.a = deselectedOpacity;
				menuColumn[i].image.color = col;
			}
		}

		// Start Button Handler

		StartCoroutine (Do_HandlerLoop);
	}

	//private void Update ()
	//{
	//	if (InputKeyHandler.IKH_Instance.Up_Start ())
	//	{
	//		GoToButton_Above ();
	//		return;
	//	}

	//	if (InputKeyHandler.IKH_Instance.Down_Start ())
	//	{
	//		GoToButton_Below ();
	//		return;
	//	}

	//	if (InputKeyHandler.IKH_Instance.Interact_Start ())
	//	{
	//		menuColumn[currentIndex].buttonEvent.Invoke ();
	//		return;
	//	}
	//}

	private void GoToButton_Above ()
	{
		if (currentIndex == 0) return;

		// Button Sound

		_buttonSounds.Play_HoverSound ();

		try
		{
			SelectNextButton_SR (-1);
		}
		catch
		{
			SelectNextButton_IMG (-1);
		}
	}

	private void GoToButton_Below ()
	{
		if (currentIndex >= menuColumn.Length - 1) return;

		// Button Sound

		_buttonSounds.Play_HoverSound ();

		try
		{
			SelectNextButton_SR (1);
		}
		catch
		{
			SelectNextButton_IMG (1);
		}
	}

	private void SelectNextButton_SR (int indexChange)
	{
		// Deselect Current Button

		Color col = menuColumn[currentIndex].spriteRenderer.color;
		col.a = deselectedOpacity;
		menuColumn[currentIndex].spriteRenderer.color = col;

		// Select New Button

		currentIndex += indexChange;

		col = menuColumn[currentIndex].spriteRenderer.color;
		col.a = selectedOpacity;
		menuColumn[currentIndex].spriteRenderer.color = col;
	}

	private void SelectNextButton_IMG (int indexChange)
	{
		// Deselect Current Button

		Color col = menuColumn[currentIndex].image.color;
		col.a = deselectedOpacity;
		menuColumn[currentIndex].image.color = col;

		// Select New Button

		currentIndex += indexChange;

		col = menuColumn[currentIndex].image.color;
		col.a = selectedOpacity;
		menuColumn[currentIndex].image.color = col;
	}
}
