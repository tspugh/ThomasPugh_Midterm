using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnd : BulletBehaviour
{

    private Vector3 facedir;

    public GameStatus gameStatus;

    public bool hitBounds = false;

    public override void Translate()
    {
        if(velocity.magnitude!=0)
        {
            facedir = velocity;
        }
        if (InBounds())
        {
            transform.position += velocity * Time.deltaTime;
            velocity += (acceleration) * Time.deltaTime;
            acceleration += jerk * Time.deltaTime;
        }
        else
        {
            hitBounds = true;
        }
    }

    public override void Rotate()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(facedir.y, facedir.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    protected bool InBounds()
    {
        return (transform.position.x > gameStatus.minX && transform.position.x < gameStatus.maxX && transform.position.y > gameStatus.minY && transform.position.y < gameStatus.maxY);
    }

}
