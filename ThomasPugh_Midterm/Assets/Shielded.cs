using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielded : MonoBehaviour
{

    public GameObject shield;

    public string[] damageableTags;

    public int damageableHealth;

    public float rotateSpeed = 30;

    public bool transferMaterial = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject s = Instantiate(shield, transform.position, Quaternion.identity) as GameObject;
        if(transferMaterial)
        {
            s.GetComponent<SpriteRenderer>().material = this.GetComponent<SpriteRenderer>().material;
        }
        ShieldScript ss = s.GetComponent<ShieldScript>();
        ss.rotateSpeed = this.rotateSpeed;
        ss.theProtected = this.gameObject;
        Damageable damageable = s.GetComponent<Damageable>();
        damageable.maxHealth = this.damageableHealth;
        damageable.health = this.damageableHealth;
        damageable.tags = this.damageableTags;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
