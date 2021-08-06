using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType { NULL, Gold }

public class LootCreature : MonoBehaviour
{
	public CircleCollider2D creatureCollider;

	public LootType[] drops;

	public HealthSystem health;
}