using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
	[SerializeField]
	private GameObject continueButton;

	[SerializeField]
	private JSONeer jSONeer;

	private DataContainer data;

	// Start Screen UI

	[SerializeField]
	private TMP_Text _title;
	[SerializeField]
	private Image[] _buttonImage;

	// Audio

	[SerializeField]
	private AudioSource[] _buttonSounds;

	// Coroutines

	private IEnumerator Do_StartMenuLoop { get { return StartMenuLoop (); } }

	private IEnumerator StartMenuLoop ()
	{
		// !!! TEMPORARY !!!
		if (InputKeyHandler.IKH_Instance == null)
		{
			GameObject ikh = new GameObject { name = "Input Key Handler" };
			ikh.AddComponent<InputKeyHandler> ();
		}

		//while (enabled)
		//{
		//	if (InputKeyHandler.IKH_Instance.Up_Start ())
		//	{
		//		print ("Pushed Up!");
		//	}

		//	if (InputKeyHandler.IKH_Instance.Down_Start ())
		//	{
		//		print ("Pushed Down!");
		//	}

		//	yield return null;
		//}

		yield return null;
	}

	private void Start ()
	{
		// !!! TEMPORARY !!!
		
		StartCoroutine (Do_StartMenuLoop);

		// Check for Previous Save Data
		// If no Data, disable Continue Button

		try
		{
			data = jSONeer.DataContainer_From_JSON ();

			if (data.dungeonHunt_DataBranches[0].currHP > 0)
			{
				continueButton.SetActive (true);
			}
		}
		catch
		{
			// Do Nothing
		}
	}

	public void New_Game_Seq ()
	{
		IEnumerator do_New_Game_Seq = Do_New_Game_Seq ();
		StartCoroutine (do_New_Game_Seq);
	}

	private IEnumerator Do_New_Game_Seq ()
	{
		foreach (AudioSource btnSound in _buttonSounds)
		{
			while (btnSound.isPlaying)
			{
				yield return null;
			}
		}

		New_Game ();
	}

	private void New_Game ()
	{
		SceneManager.LoadScene ("Opening Cut Scene");
	}

	public void Load_Game_Seq ()
	{
		IEnumerator do_Load_Game_Seq = Do_Load_Game_Seq ();
		StartCoroutine (do_Load_Game_Seq);
	}

	private IEnumerator Do_Load_Game_Seq ()
	{
		foreach (AudioSource btnSound in _buttonSounds)
		{
			while (btnSound.isPlaying) { yield return null; }
		}

		Load_Game ();
	}

	private void Load_Game ()
	{
		//DataContainer dataContainer = jSONeer.DataContainer_From_JSON ();

		// Health

		DataPasser.DPInstance.playerCurrHp = data.dungeonHunt_DataBranches[0].currHP;
		DataPasser.DPInstance.playerFullHp = data.dungeonHunt_DataBranches[0].fullHP;

		// Armor

		switch (data.dungeonHunt_DataBranches[0].armorType)
		{
			case "Gold":
				DataPasser.DPInstance.SetArmorType (ArmorType.Gold);
				break;
			case "Bronze":
				DataPasser.DPInstance.SetArmorType (ArmorType.Bronze);
				break;
			case "Ruby":
				DataPasser.DPInstance.SetArmorType (ArmorType.Ruby);
				break;
			default:
				DataPasser.DPInstance.SetArmorType (ArmorType.NULL);
				break;
		}

		// Attack

		DataPasser.DPInstance.Set_CurrDamage (data.dungeonHunt_DataBranches[0].damage);

		switch (data.dungeonHunt_DataBranches[0].attackEffect)
		{
			case "DOT":
				DataPasser.DPInstance.currAttackEffect = Attack_Effect.DOT;
				break;
			default:
				DataPasser.DPInstance.currAttackEffect = Attack_Effect.NULL;
				break;
		}

		// Items

		// Gold

		DataPasser.DPInstance.Change_GoldCount (data.dungeonHunt_DataBranches[0].goldCount);

		// Rooms

		DataPasser.DPInstance.previousRoom = data.dungeonHunt_DataBranches[0].previousRoom;

		// Bosses

		foreach (string bossName in data.dungeonHunt_DataBranches[0].defeatedBosses)
		{
			switch (bossName)
			{
				case "GreatBeholder":
					DataPasser.DPInstance.defeatedBosses.Add (BossType.GreatBeholder);
					break;
				case "SkeleMech":
					DataPasser.DPInstance.defeatedBosses.Add (BossType.SkeleMech);
					break;
			}
		}

		// Data Converted, Start the Game

		SceneManager.LoadScene (data.dungeonHunt_DataBranches[0].currRoom);
	}

	public void Hide_StartScreen ()
	{
		_title.gameObject.SetActive (false);

		foreach (Image image in _buttonImage)
		{
			image.enabled = false;
			image.transform.GetChild (0).gameObject.SetActive (false);
		}
	}
}
