using UnityEngine;
using System.Collections;

public class BulletBasic : BulletBehaviour
{
	public override void Translate()
    {
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.y,velocity.x)*Mathf.Rad2Deg, Vector3.forward);
        transform.position += velocity * Time.deltaTime;
        velocity += (acceleration) * Time.deltaTime;
        acceleration += jerk * Time.deltaTime;
    }
}

