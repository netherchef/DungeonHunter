using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAttack : MonoBehaviour
{
    private bool lazerHit;
    [SerializeField]
    private int lazerDamage = 3;

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            if (!lazerHit)
            {
                lazerHit = true;

                other.transform.GetComponent<HealthSystem> ().GetHurt (lazerDamage);
            }
        }
    }

    public bool LazerHit () { return lazerHit; }

    public void Reset () { lazerHit = false; }
}
