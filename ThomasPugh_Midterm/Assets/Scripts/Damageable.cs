using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public float health;
    public float maxHealth;
    public int pointValue;
    public string[] tags;
    public float damageTaken;
    public AudioClip destructionNoise;
    public AudioClip damageNoise;

    public bool hasInvinciblility = false;
    public float invincibilityTime;

    private bool isInvincibile;

    private void Start()
    {
        isInvincibile = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        foreach(string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag) && !isInvincibile)
            {
                if(hasInvinciblility)
                    StartCoroutine(InvincibleTimer());
                Damage(damageTaken);
                if(damageNoise!=null)
                    AudioSource.PlayClipAtPoint(damageNoise, Camera.main.transform.position + new Vector3(0f, 0f, 1f), 0.125f);
                if (tag.Contains("Bullet"))
                    Destroy(collision.gameObject);
            }
        }
    }

    public void DoDestroy()
    {
        GameEvents.InvokeIncreaseScore(pointValue, gameObject.CompareTag("Player"));
        GameEvents.InvokeDamagableDestroyed(gameObject);
        AudioSource.PlayClipAtPoint(destructionNoise, Camera.main.transform.position + new Vector3(0f, 0f, 1f), 0.125f);
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }

    public void SetHealth(float health)
    {
        this.health = health;
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

    public IEnumerator InvincibleTimer()
    {
        isInvincibile = true;
        float time = invincibilityTime;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        UnityEngine.Color c = sr.color;
        while(time>=0)
        {
            time -= Time.deltaTime;
            c.a = Mathf.Min(1,(invincibilityTime - time) / invincibilityTime);
            sr.color = c;
            yield return null;
        }
        isInvincibile = false;
    }
}
