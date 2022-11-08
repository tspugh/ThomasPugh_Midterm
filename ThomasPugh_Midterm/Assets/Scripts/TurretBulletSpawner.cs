using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBulletSpawner : BulletSpawner
{
    public override IEnumerator InstantiateBullets(BulletPattern pat)
    {
        Vector3 direction = -GetComponent<TurretScript>().GetDirectionToFire();

        float angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
        if (direction.x < 0)
            angle = 180 - angle;

        if (pat.randomAngle)
            angle = UnityEngine.Random.Range(0, 360);



        float rotationS = pat.rotationSpeed;
        float rotationA = pat.rotationAccel;
        float t = pat.duration;
        float interv = pat.interval;

        float bS = pat.bulletS;
        float bA = pat.bulletA;
        float bJ = pat.bulletJ;

        int aOB = pat.amountOfBullets;
        int dAOB = pat.deltaAmountOfBullets;

        AudioSource audioSource = GetComponent<AudioSource>();
        float pitch = 1;

        while (isSpawningBullets && t > 0)
        {
            yield return new WaitForSeconds(interv);


            if (audioSource)
            {
                audioSource.clip = pat.mySound;
                audioSource.pitch = pitch;
                audioSource.volume = 0.125f;
                audioSource.Play();
            }

            for (int i = 1; i <= aOB; i++)
            {
                float ang = (angle - 0.5f * pat.rangeInDegrees + i * pat.rangeInDegrees / (float)aOB) * Mathf.Deg2Rad;
                Vector3 norm = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0) * -1f;
                GameObject o = Instantiate(pat.bullet, transform.position, Quaternion.identity) as GameObject;
                o.SendMessage("InitializeBullet", new BulletInitial(bS * norm, bA * norm, bJ * norm));
            }

            bS += pat.deltaS * interv;
            bA += pat.deltaA * interv;
            bJ += pat.deltaJ * interv;

            angle += rotationS * interv;
            rotationS += rotationA * interv;
            rotationA += pat.rotationJerk * interv;

            pitch += pat.deltaPitch;

            if (aOB < 1)
                dAOB = -dAOB;
            aOB += dAOB;

            direction = -GetComponent<TurretScript>().GetDirectionToFire();
            angle = Mathf.Asin(direction.y / direction.magnitude) * Mathf.Rad2Deg;
            if (direction.x < 0)
                angle = 180 - angle;

            interv = Mathf.Clamp(interv + pat.deltaInterval, 0.01f, Mathf.Infinity);
            t -= interv;
        }
        yield return null;
    }
}
