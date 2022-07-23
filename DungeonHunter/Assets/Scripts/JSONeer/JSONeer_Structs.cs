using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct DataContainer
{
	public List<DataBranch> dungeonHunt_DataBranches;
}

[System.Serializable]
public struct DataBranch
{
	public int currHP;
	public int fullHP;

	public string armorType;

	public int damage;
	public string attackEffect;

	public string[] items;

	public int goldCount;

	public string currRoom;
	public string previousRoom;
}