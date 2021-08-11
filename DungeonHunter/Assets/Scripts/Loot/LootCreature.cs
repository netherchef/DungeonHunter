using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCreature : MonoBehaviour
{
	public CircleCollider2D creatureCollider;

	public ItemType[] drops;

	public HealthSystem health;
}