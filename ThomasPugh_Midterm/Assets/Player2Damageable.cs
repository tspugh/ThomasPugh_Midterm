using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2Damageable : Damageable
{

    public float bigDamage = 15;

    public override void Damage(float damage)
    {
        base.Damage(damage);
        DamageAll(bigDamage*(maxHealth-health)*2/maxHealth+1);

    }

    public void DamageAll(float damage)
    {
        GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(GameObject o in holder)
        {
            if(o.CompareTag("Enemy"))
            {
                o.GetComponent<Damageable>().Damage((int)damage);
            }
            else if (o.CompareTag("EnemyBullet"))
            {
                Destroy(o);
            }
        }
    }
}
