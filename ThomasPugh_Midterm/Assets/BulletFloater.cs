using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletFloater : BulletBasic
{
    public float bulletSurvivalTime;
    public float decelrate = 5;

    // Start is called before the first frame update
    new void Start()
    {
        StartCoroutine(DeathCountdown());
    }

    // Update is called once per frame
    public new void Update()
    {
        if (velocity.magnitude > 0.00001)
            acceleration = -1 * decelrate * velocity.normalized;
        else
            acceleration = Vector3.zero;
        base.Update();
        
    }

    public virtual void OnDeath()
    {

    }

    public IEnumerator DeathCountdown()
    {
        float time = 0;
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        Light2D light = GetComponent<Light2D>();
        Color initialcolor = r.color;
        float lightinitial = light.intensity;
        while (time<bulletSurvivalTime)
        {
            r.color = new Color(initialcolor.r, initialcolor.g, initialcolor.b, Mathf.Pow(Mathf.Lerp(1, 0, time / bulletSurvivalTime), 5f/6f));
            light.intensity = Mathf.Pow(Mathf.Lerp(lightinitial, 0, time / bulletSurvivalTime), 5f / 6f);
            time += Time.deltaTime;
            yield return null;
        }
        OnDeath();
        Destroy(gameObject);
    }
}
