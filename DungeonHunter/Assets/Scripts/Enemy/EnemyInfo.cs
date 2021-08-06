using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
	[Header ("Components:")]

	public EnemyAnimatorFunctions enemyAnimatorFunctions;

	[Header ("Variables:")]

	public float attackRange = 0.6f;

	public float windUp = 0.5f;
	public float attackDuration = 0.5f;
	public float coolDown = 0.2f;

	public float moveSpeed = 2f;
}
