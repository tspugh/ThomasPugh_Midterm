using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableSubCollider : MonoBehaviour
{
    public GameObject damaged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damaged.GetComponent<Damageable>().DoCollision(collision);
    }
}
