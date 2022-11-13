using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySineMover : EnemyBasic
{
    public float maxChange;
    public float minPeriod;
    public float maxPeriod;

    private float time;
    private float period;

    protected new void Start()
    {
        base.Start();
        time = 0;
        period = Random.Range(minPeriod, maxPeriod);
    }


    public override void Translate()
    {
        base.Translate();
        vel += maxChange * Mathf.Sin(time * Mathf.PI * 2 / period) * Time.deltaTime;
        time += Time.deltaTime;
    }
}
