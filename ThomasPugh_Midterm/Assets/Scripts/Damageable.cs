using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public string[] tags;
    public float damageTaken;

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        foreach(string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                Debug.Log(tag);
                Damage(damageTaken);
                if (tag.Contains("Bullet"))
                    Destroy(collision.gameObject);
            }
        }
    }

    public void DoDestroy()
    {
        GameEvents.InvokeDamagableDestroyed(gameObject);
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }

    public bool HealthCheck()
    {
        if (health <= 0)
        {
            DoDestroy();
            return false;
        }
        else
        {
            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
    }
}
