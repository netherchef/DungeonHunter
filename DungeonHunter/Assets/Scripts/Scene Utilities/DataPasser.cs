using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPasser : MonoBehaviour
{
	/// This is a private instance of this class
	/// which we should only have 1 of in each scene.

	private static DataPasser _instance;

	/// This is a public copy of the above instance.
	/// We use this to access the instance's functionality.

	public static DataPasser DPInstance { get { return _instance; } }

	private void Awake ()
	{
		/// This is run on every instance of this script.
		/// If a private instance exists, but it is not
		/// the one we preserve with DontDestroyOnLoad, detroy it.

		if (_instance != null && _instance != this)
		{
			Destroy (gameObject);
		}
		else
		{
			_instance = this;
		}

		// Preserve this game object between scenes.

		DontDestroyOnLoad (this);
	}

	/// Normal variables we access through functions like 
	/// DataPasser.DPInstance.Set_Variable ().

	#region Player _____________________________________________________________

	public int playerCurrHp;
	public int playerFullHp;
	
	private ArmorType armorType;

	public int currDamage;
	public Attack_Effect currAttackEffect;

	public ItemType[] inventoryItems;

	public ArmorType CurrentArmorType ()
	{
		return armorType;
	}

	public void SetArmorType (ArmorType newType)
	{
		armorType = newType;
	}

	#endregion

	#region Room ______________________________________________________________

	public string previousRoom;
	//public DoorDirection previousDoorDir;

	#endregion

	#region ____________________________________________________________________

	public List<BossType> defeatedBosses = new List<BossType> ();

	public void RecordBossDefeat (BossType bossType)
	{
		defeatedBosses.Add (bossType);
	}

	public bool IsDefeated(BossType bossType)
	{
		foreach (BossType boss in defeatedBosses)
		{
			if (boss == bossType) return true;
		}

		return false;
	}

	#endregion
}