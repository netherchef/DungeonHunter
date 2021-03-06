using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMeter : MonoBehaviour
{
	// Singleton

	private static GoldMeter _instance;

	public static GoldMeter GMInstance { get { return _instance; } }
	
	[Header ("Components:")]

	[SerializeField]
	private GameObject goldPrefab;

	// Variables
	//[SerializeField]
	//private int currGold;
	[SerializeField]
	private int furthest;
	private const float offset = 0.5f;

	private void Awake()
	{
		// Singleton

		if (_instance != null & _instance != this) Destroy(gameObject);
		else _instance = this;

		DontDestroyOnLoad(this);
	}

	public void AddGold ()
	{
		Vector3 newPos = 
			DataPasser.DPInstance.CurrentGold () <= 0 ? transform.position :
			transform.GetChild(furthest).position + new Vector3(offset, 0);

		GameObject newGold = Instantiate (
			goldPrefab, 
			newPos, 
			Quaternion.identity, 
			transform);

		if (DataPasser.DPInstance.CurrentGold () > 0) furthest++;
		DataPasser.DPInstance.Change_GoldCount (1);
	}

	public void MinusGold (int amount = 1)
	{
		if (DataPasser.DPInstance.CurrentGold () > 0)
		{
			Destroy(transform.GetChild(furthest).gameObject);

			if (furthest > 0) furthest--;
			DataPasser.DPInstance.Change_GoldCount (-1);
		}
	}

	//public int CurrentGold () { return currGold; }

	//public bool addGold, minusGold;

	//private void Update()
	//{
	//	if (addGold)
	//	{
	//		addGold = false;

	//		AddGold ();
	//	}

	//	if (minusGold)
	//	{
	//		minusGold = false;

	//		MinusGold ();
	//	}
	//}
}