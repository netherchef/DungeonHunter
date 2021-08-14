using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { NULL, Gold, PotionOfHealth }

public class Item : MonoBehaviour
{
	public ItemType type;
}