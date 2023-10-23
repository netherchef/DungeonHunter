using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHandler : MonoBehaviour
{
	public GameObject goldMeterPrefab;
	public HealthSystem playerHealth;
	public PlayerAttack playerAttack;
	public HealthBar healthBar;
	public PlayerInventory playerInventory;
	public DoorHandler doorHandler;
	public Transform player;
	public EnemyHandler enemyHandler;
	private CameraController camControl;

	[SerializeField]
	private Shop shop;

	// Coroutines

	private IEnumerator Entry;
	private IEnumerator PostEntry;

	private void Start ()
	{
		Entry = DoEntry ();
		StartCoroutine (Entry);
	}

	private IEnumerator DoEntry ()
	{
		// Prep ////////////////////////////////////////////////////////////////

		// Data Passer

		// !!! TEMPORARY !!!
		// If there is NO Data Passer, create one.

		if (DataPasser.DPInstance == null)
		{
			GameObject dataPasser = new GameObject { name = "Data Passer" };
			dataPasser.AddComponent<DataPasser> ();
		}

		// If there is NO Gold Meter, create one.

		if (GoldMeter.GMInstance == null)
		{
			//print ("Gold Meter Created!");

			Instantiate (goldMeterPrefab);
			GoldMeter.GMInstance.UpdateGoldDisplay_OnGameLoad ();
		}

		// Player Health

		if (DataPasser.DPInstance.playerCurrHp == 0) DataPasser.DPInstance.playerCurrHp = playerHealth.currHp;
		else playerHealth.currHp = DataPasser.DPInstance.playerCurrHp;

		// Player Damage & Attack Effect

		if (DataPasser.DPInstance.currDamage != 0)
			playerAttack.SetDamage (DataPasser.DPInstance.currDamage);

		if (DataPasser.DPInstance.currAttackEffect != Attack_Effect.NULL)
			playerAttack.Set_AttackEffect (DataPasser.DPInstance.currAttackEffect);

		// Input Key Handler

		if (InputKeyHandler.IKH_Instance == null)
		{
			GameObject ikh = new GameObject { name = "Input Key Handler" };
			ikh.AddComponent<InputKeyHandler> ();

			Debug.Log ("Creating 'Input Key Handler'.");
		}

		// Player Armor

		switch (DataPasser.DPInstance.CurrentArmorType ())
		{
			case ArmorType.Gold:
				player.GetComponentInChildren<PlayerAnimator> ().Set_Gold ();
				break;
			case ArmorType.Bronze:
				player.GetComponentInChildren<PlayerAnimator> ().Set_Bronze ();
				break;
			case ArmorType.Ruby:
				player.GetComponentInChildren<PlayerAnimator> ().Set_Ruby ();
				break;
			default:
				break;
		}

		// Health Bar

		healthBar.Prep (playerHealth);

		// Player Inventory

		//playerInventory.AssignGoldMeter ();

		if (DataPasser.DPInstance.inventoryItems != null)
		playerInventory.AddItems (DataPasser.DPInstance.inventoryItems);

		// Door Handler

		doorHandler.Prep ();

		// Position Player at Door

		while (camControl == null)
		{
			try
			{
				camControl = GameObject.Find ("Main Camera").GetComponent<CameraController> ();
			}
			catch
			{
			}

			yield return null;
		}

		if (DataPasser.DPInstance.previousRoom != "")
		{
			Vector3 newPos = doorHandler.DoorPosition(DataPasser.DPInstance.previousRoom);

			float displacement = 0.5f;

			if (newPos.x > 0)
			{
				newPos += new Vector3 (-displacement, 0); // Left
				camControl.direction = CamDirection.Left;
			}
			else if (newPos.x < 0)
			{
				newPos += new Vector3 (displacement, 0); // Right
				camControl.direction = CamDirection.Right;
			}
			else if (newPos.y > 0)
			{
				newPos += new Vector3 (0, -displacement); // Down
				camControl.direction = CamDirection.Down;
			}
			else
			{
				newPos += new Vector3 (0, displacement); // Up
				camControl.direction = CamDirection.Up;
			}

			player.position = newPos;
		}

		// Shop

		if (shop) shop.Prep (player.GetComponentInChildren<PlayerAnimator> ());

		// Spawn Enemies

		if (enemyHandler) enemyHandler.Prep ();

		// Check Ambience according to Current Scene

		BackgroundAudio.BGMA_Instance.Check_BackgroundAudio ();

		// Execute /////////////////////////////////////////////////////////////

		// Door Handler

		doorHandler.Execute ();
		
		// Enable Pause Menu

		if (!PauseMenu.PMInstance)
		{
			GameObject pauseMenu = new GameObject ("Pause Menu");
			pauseMenu.AddComponent<PauseMenu> ();

			Debug.Log ("Creating 'Pause Menu'.");
		}

		if (!PauseMenu.PMInstance.Is_Released ()) PauseMenu.PMInstance.Release ();

		// !!! TEMPORARY !!!
		GameObject.Find ("Jake").GetComponent<PlayerInputHandler> ().Execute ();

		PostEntry = DoPostEntry ();
		StartCoroutine (PostEntry);
	}

	private IEnumerator DoPostEntry ()
	{
		camControl.Execute ();
		while (camControl.IsActive ()) yield return null;

		if (enemyHandler) enemyHandler.Execute ();
	}
}