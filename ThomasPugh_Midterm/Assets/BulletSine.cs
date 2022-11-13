using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSine : BulletBasic
{
    public float amplitude;
    public float period;

    private float time;

    public override void DoOnStart()
    {
        time = 0;
    }


    // Update is called once per frame
    public override void Translate()
    {
        base.Translate();
        Vector2 perp = Vector2.Perpendicular(new Vector2 (velocity.x, velocity.y));
        transform.position += Mathf.Sin(time * Mathf.PI * 2 / period) * new Vector3(perp.x, perp.y, 0) * amplitude * Time.deltaTime;
        time += Time.deltaTime;
    }
}
