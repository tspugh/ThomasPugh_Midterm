using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnpredictable : EnemyBasic
{

    public float rotateMinInterval, rotateMaxInterval;
    public float rotateMinDuration, rotateMaxDuration;

    private float time;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetRandomTime();
        accelerationDir = Vector3.zero;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (snakeHead == null)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                SetRandomTime();
                float t = Random.Range(rotateMinDuration, rotateMaxDuration);
                StartCoroutine(RotateMe(t));
                time += t;
            }

            Vector3 velocitynew = vel * velocityDir + accel * accelerationDir * Time.deltaTime;
            if (velocitynew.magnitude != 0)
            {
                vel = velocitynew.magnitude;
                velocityDir = velocitynew.normalized;
            }
        }

        base.Update();
    }

    public override void Rotate()
    {
        transform.rotation = Quaternion.AngleAxis(-90f + Mathf.Atan2(velocityDir.y, velocityDir.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    public void SetRandomTime()
    {
        time = Random.Range(rotateMinInterval, rotateMaxInterval);
    }

    public IEnumerator RotateMe(float duration)
    {
        float t = 0;
        float dir = (Random.Range(0, 2) == 0) ? -1 : 1;
        while (t<duration)
        {
            yield return null;
            Vector2 perp = Vector2.Perpendicular(new Vector2(velocityDir.x, velocityDir.y)) * dir;
            accelerationDir = new Vector3(perp.x, perp.y, 0);
            t += Time.deltaTime;
        }
        accelerationDir = Vector3.zero;
    }
}
