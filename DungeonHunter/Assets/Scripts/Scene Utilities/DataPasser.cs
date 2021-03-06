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

	// Health

	public int playerCurrHp; // Marked for Save
	public int playerFullHp; // Marked for Save

	// Armor

	private ArmorType armorType; // Marked for Save

	// Attack

	public int currDamage; // Marked for Save
	public Attack_Effect currAttackEffect; // Marked for Save

	// Items

	public ItemType[] inventoryItems; // Marked for Save

	public ArmorType CurrentArmorType () { return armorType; }
	public void SetArmorType (ArmorType newType) { armorType = newType; }

	public void Set_CurrDamage (int value) { currDamage = value; }

	public void Set_Items (ItemType[] items) { inventoryItems = items; }

	private int currGold;

	public int CurrentGold () { return currGold; }
	public void Change_GoldCount (int val) { currGold += val; }

	#endregion

	#region Room ______________________________________________________________

	public string previousRoom; // Marked for Save

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