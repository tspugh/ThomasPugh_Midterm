using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnSpawn : MonoBehaviour
{
    SpriteRenderer sr;
    Collider2D c2d;
    EnemyBehaviour ebh;
    BulletSpawner bs;
    Damageable dmg;
    ParticleSystem ps;

    public float predelay;
    public AudioClip spawnSound;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        c2d = GetComponent<Collider2D>();
        ebh = GetComponent<EnemyBehaviour>();
        bs = GetComponent<BulletSpawner>();
        dmg = GetComponent<Damageable>();
        ps = GetComponent<ParticleSystem>();

        
        


    }

    private void Start()
    {
        if (ps != null && predelay == 0)
            predelay = ps.main.duration;

        StartCoroutine(DelayAwake(predelay));

    }

    public void SetPredelay(float predelaye)
    {
        StopAllCoroutines();
        if (ps != null && predelay == 0)
            predelay = ps.main.duration;
        this.predelay = predelaye;

        StartCoroutine(DelayAwake(predelay));
    }

    public float GetPredelay()
    {
        if (ps != null && predelay == 0)
            predelay = ps.main.duration;
        return predelay;
    }

    public IEnumerator DelayAwake(float time)
    {
        SetAwake(false);
        yield return new WaitForEndOfFrame();
        
        if (spawnSound!=null)
        {
            AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position + new Vector3(0f, 0f, 1f), 0.100f);
        }
        yield return new WaitForSeconds(time);
        SetAwake(true);

    }

    public void SetAwake(bool awake)
    {
        if (sr != null)
            sr.enabled = awake;
        if (c2d != null)
            c2d.enabled = awake;
        if (ebh != null)
            ebh.enabled = awake;
        if (bs != null)
            bs.enabled = awake;
        if (dmg != null)
            dmg.enabled = awake;
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(awake);
        }
    }
}
