using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGravitator : EnemyBasic
{

    public float playerVisionRadius = 20;
    public GameObject playerRangeDetector;
    PlayerDetector pd;

    public bool away = false;

    private float a;

    public override void Start()
    {
        base.Start();
        pd = playerRangeDetector.GetComponent<PlayerDetector>();
        pd.SetRadius(playerVisionRadius);
    }

    public override void Rotate()
    {
        transform.rotation = Quaternion.AngleAxis(-90f + Mathf.Atan2(velocityDir.y, velocityDir.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    public override void Update()
    {
        base.Update();
        if (snakeHead == null)
        {
            UpdateAccel();
        }
    }

    public void UpdateAccel()
    {
        if(pd.hasPlayerPos)
        {
            Vector3 dir = (pd.playerPos - transform.position);
            accelerationDir = dir.normalized;
            a = accel * 30 / Mathf.Pow(dir.magnitude, 2);
        }
        else
        {
            if (away)
                a = 1;
            else
                a = -1;
            accelerationDir = velocityDir;
        }
        a = Mathf.Min(a, 200);
        Vector3 newspeed;
        if(away)
            newspeed = vel * velocityDir - a * accelerationDir * Time.deltaTime;
        else
            newspeed = vel * velocityDir + a * accelerationDir * Time.deltaTime;
        vel = (newspeed.magnitude < 0.0001f) ? 0 : newspeed.magnitude;
        velocityDir = newspeed.normalized;
    }
}
