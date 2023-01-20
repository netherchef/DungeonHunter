// JSONeer v0.017

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class JSONeer : MonoBehaviour
{
	[Header ("Scripts:")]

	public DataContainer container;

	[Header ("Variables:")]

	public string fileName = "DataList.json";
	public bool check;
	public bool write;

	[Header ("Debug:")]

	[SerializeField]
	private bool clearData;

	private void Update ()
	{
		// Read

		if (check)
		{
			check = false;

			container = DataContainer_From_JSON ();

			return;
		}

		// Write

		if (write)
		{
			write = false;

			Write_DataContainer_To_JSON ();
		}

		// Clear Data

		if (clearData)
		{
			clearData = false;

			container = default;
		}
	}

	#region From JSON __________________________________________________________

	public DataContainer DataContainer_From_JSON ()
	{
		// Define JSON file path

		string filePath = Application.persistentDataPath + "/" + fileName;

		try
		{
			// Pull data from the JSON file as a string

			string jsonContent = File.ReadAllText (filePath);

			try
			{
				// Convert the string from JSON and load it into a struct
				
				return JsonUtility.FromJson<DataContainer> (jsonContent);
			}
			catch
			{
				Debug.LogWarning ("JSON parsing failed.");

				return default;
			}
		}
		catch
		{
			Debug.LogWarning ("JSON file failed to load.");

			return default;
		}
	}

	#endregion

	#region To JSON ____________________________________________________________

	private void Write_DataContainer_To_JSON ()
	{
		// Define JSON file path

		string filePath = Application.persistentDataPath + "/" + fileName;

		try
		{
			// Format the struct as a JSON string

			string jsonContent = JsonUtility.ToJson (container, true);

			// Write the JSON string to persistent data

			File.WriteAllText (filePath, jsonContent);
		}
		catch
		{
			Debug.LogWarning ("JSON file failed to write.");
		}
	}

	#endregion

	public void Save (string currRoomName)
	{
		PassData_ToContainer (currRoomName);
		Write_DataContainer_To_JSON ();
	}

	private void PassData_ToContainer (string currRoomName)
	{
		if (container.dungeonHunt_DataBranches == null || container.dungeonHunt_DataBranches.Count <= 0)
		{
			container.dungeonHunt_DataBranches.Add (DataForJSONeer (currRoomName));

			return;
		}

		container.dungeonHunt_DataBranches[0] = DataForJSONeer (currRoomName);
	}

	private DataBranch DataForJSONeer (string currRoomName)
	{
		DataBranch branch = new DataBranch ();

		// Health

		branch.currHP = DataPasser.DPInstance.playerCurrHp;
		branch.fullHP = DataPasser.DPInstance.playerFullHp;

		// Armor

		branch.armorType = DataPasser.DPInstance.CurrentArmorType ().ToString ();

		// Attack

		branch.damage = DataPasser.DPInstance.currDamage;
		branch.attackEffect = DataPasser.DPInstance.currAttackEffect.ToString ();

		// Items

		string[] itemNames;

		if (DataPasser.DPInstance.inventoryItems != null && DataPasser.DPInstance.inventoryItems.Length > 0)
		{
			itemNames = new string[DataPasser.DPInstance.inventoryItems.Length];

			for (int i = 0; i < itemNames.Length; i++)
			{
				itemNames[i] = DataPasser.DPInstance.inventoryItems[i].ToString ();
			}

			branch.items = itemNames;
		}

		// Gold

		branch.goldCount = DataPasser.DPInstance.CurrentGold ();

		// Rooms

		branch.currRoom = currRoomName;
		branch.previousRoom = DataPasser.DPInstance.previousRoom;

		// Bosses

		string[] bossNames;

		if (DataPasser.DPInstance.defeatedBosses != null && DataPasser.DPInstance.defeatedBosses.Count > 0)
		{
			bossNames = new string[DataPasser.DPInstance.defeatedBosses.Count];

			for (int i = 0; i < bossNames.Length; i++)
			{
				bossNames[i] = DataPasser.DPInstance.defeatedBosses[i].ToString ();
			}

			branch.defeatedBosses = bossNames;
		}

		return branch;
	}
}