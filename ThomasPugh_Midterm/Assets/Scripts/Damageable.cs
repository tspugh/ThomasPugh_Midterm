using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public bool behaveNormally = true;

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

    private void Awake()
    {
        GameEvents.GameOver += OnGameOver;
    }

    public virtual void Start()
    {
        isInvincibile = false;
        if(health!=maxHealth)
        {
            maxHealth = Mathf.Round(Random.Range(health, maxHealth));
            health = maxHealth;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (behaveNormally)
            DoCollision(collision);
    }

    public void DoCollision(Collider2D collision)
    {
        foreach (string tag in tags)
        {
            if (collision.gameObject.CompareTag(tag) && !isInvincibile)
            {
                if (hasInvinciblility)
                    StartCoroutine(InvincibleTimer());
                Damage(damageTaken);
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

    public virtual void Damage(float damage)
    {
        health -= damage;
        if(damageNoise)
            AudioSource.PlayClipAtPoint(damageNoise, Camera.main.transform.position + new Vector3(0f, 0f, 1f), gameObject.CompareTag("Player")? 0.20f : 0.050f);
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

    public void OnGameOver(object sender, GameOverArgs e)
    {
        SetHealth(0);
    }
}
