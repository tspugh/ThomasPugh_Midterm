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

    public Vector3 velocity, acceleration, jerk;

    public virtual void Translate()
    { }



    // Start is called before the first frame update
    void Awake()
    {
        InitializeBullet(new BulletInitial(Vector3.zero, Vector3.zero, Vector3.zero));
    }

    public virtual void Rotate()
    { }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Translate();
        if (Bounds.IsOutsideBounds(transform.position, 50f))
            Destroy(gameObject);

    }

    public void InitializeBullet(BulletInitial init)
    {
        this.velocity = init.vel;
        this.acceleration = init.accel;
        this.jerk = init.jer;
    }
}
