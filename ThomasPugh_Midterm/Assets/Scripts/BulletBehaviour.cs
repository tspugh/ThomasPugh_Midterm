using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInitial
{
    public Vector3 vel, accel, jer;
    public BulletInitial(Vector3 vel, Vector3 accel, Vector3 jer)
    {
        this.vel = vel;
        this.accel = accel;
        this.jer = jer;
    }
}

public class BulletBehaviour : MonoBehaviour
{
    public float bulletBounds = 50;
    public Vector3 velocity, acceleration, jerk;

    public virtual void Translate()
    { }



    // Start is called before the first frame update
    public virtual void Awake()
    {
        InitializeBullet(new BulletInitial(Vector3.zero, Vector3.zero, Vector3.zero));
    }

    public void Start()
    {
        DoOnStart();
    }

    public virtual void Rotate()
    { }

    public virtual void DoOnStart()
    { }

    // Update is called once per frame
    public void Update()
    {
        Rotate();
        Translate();
        if (Bounds.IsOutsideBounds(transform.position, bulletBounds))
            Destroy(gameObject);

    }

    public void InitializeBullet(BulletInitial init)
    {
        this.velocity = init.vel;
        this.acceleration = init.accel;
        this.jerk = init.jer;
    }
}
