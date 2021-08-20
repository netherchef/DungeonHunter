using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Heart
{
	public GameObject heartObject;
	public SpriteRenderer sr;
	public bool empty;
}

public class HealthBar : MonoBehaviour
{
	[Header ("Components:")]

	public Transform heartContainer;

	public GameObject heartPrefab;

	public Sprite fullHeartSprite;
	public Sprite emptyHeartSprite;

	//[Header ("Scripts:")]

	//public HealthSystem playerHealth;

	// Variables

	public List<Heart> hearts = new List<Heart> ();

	private float displacement = 0.35f;

	//[Header ("Debug:")]

	//public bool addHeart;
	//public bool removeHeart;
	//public bool fill;
	//public bool drain;

	public void Prep (HealthSystem playerHealth)
	{
		InitialiseHearts (playerHealth.fullHp, playerHealth.currHp);
	}

	//#if UNITY_EDITOR
	//	private void Update ()
	//	{
	//		if (addHeart)
	//		{
	//			addHeart = false;
	//			AddHeart ();
	//		}
	//		else if (removeHeart)
	//		{
	//			removeHeart = false;
	//			RemoveHeart ();
	//		}

	//		if (fill)
	//		{
	//			fill = false;
	//			FillHeart ();
	//		}
	//		else if (drain)
	//		{
	//			drain = false;
	//			DrainHeart ();
	//		}
	//	}
	//#endif

	public void FillHeart ()
	{
		for (int h = 0; h < hearts.Count; h++)
		{
			if (hearts[h].empty)
			{
				Heart tempHeart = hearts[h];
				tempHeart.sr.sprite = fullHeartSprite;
				tempHeart.empty = false;
				hearts[h] = tempHeart;

				return;
			}
		}
	}

	public void DrainHeart ()
	{
		for (int h = hearts.Count; h > 0; h--)
		{
			if (!hearts[h - 1].empty)
			{
				Heart tempHeart = hearts[h - 1];
				tempHeart.sr.sprite = emptyHeartSprite;
				tempHeart.empty = true;
				hearts[h - 1] = tempHeart;

				return;
			}
		}
	}

	private void InitialiseHearts (int fullHp, int currHp)
	{
		int emptyHearts = fullHp - currHp;

		for (int i = currHp; i > 0; i--) IncreaseHeart ();
		for (int e = emptyHearts; e > 0; e--) IncreaseHeart (true);
	}

	public void AddHeart (int count = 1)
	{
		for (int i = 0; i < count; i++) IncreaseHeart ();
	}

	private void IncreaseHeart (bool empty = false)
	{
		GameObject newHeart = Instantiate (heartPrefab, heartContainer);

		if (hearts.Count <= 0) newHeart.transform.localPosition = heartContainer.localPosition;
		else newHeart.transform.localPosition = hearts[hearts.Count - 1].heartObject.transform.localPosition + new Vector3 (displacement, 0);

		if (!empty) hearts.Add (new Heart { heartObject = newHeart, sr = newHeart.GetComponent<SpriteRenderer> (), empty = false });
		else
		{
			Heart tempHeart = new Heart { heartObject = newHeart, sr = newHeart.GetComponent<SpriteRenderer> (), empty = true };
			tempHeart.sr.sprite = emptyHeartSprite;
			hearts.Add (tempHeart);
		}
	}

	public void RemoveHeart (int count = 1)
	{
		for (int i = 0; i < count; i++) DecreaseHeart ();
	}

	private void DecreaseHeart ()
	{
		try
		{
			int index = hearts.Count - 1;

			Destroy (hearts[index].heartObject);
			hearts.Remove (hearts[index]);
		}
		catch
		{
			if (hearts.Count <= 0) Debug.LogWarning ("No hearts left!");
		}
	}
}
