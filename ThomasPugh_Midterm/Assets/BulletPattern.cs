using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class Trigger
{

}

[CreateAssetMenu(menuName="BulletPattern")]
public class BulletPattern : ScriptableObject
{
    public GameObject bullet; //the bullet to be created
    public float bulletS, bulletA, bulletJ; //the bullet speed, accel, and jerk respectively
    public float deltaS, deltaA, deltaJ;
    public Vector3 myPosition;
    public float rangeInDegrees; //the spread in which bullets will be created (>=360 is full range)
    public Vector3 direction; //the vector pointing where the center of the range should go
    public int amountOfBullets; //how many bullets are created in one fire

    public float interval; // the time between each full spread of bullets
    public float deltaInterval;
    public float duration; // the duration in total of how long bullets are spawned

    public float rotationSpeed; // how fast the spawner rotates
    public float rotationAccel; // how fast the 
    public float rotationJerk;

    public bool isRunning = true;

    public IEnumerator InstantiateBullets()
    {
        float angle = Vector3.Angle(direction,Vector3.right);
        float rotationS = rotationSpeed;
        float rotationA = rotationAccel;
        float t = duration;
        float interv = interval;

        float bS = bulletS;
        float bA = bulletA;
        float bJ = bulletJ;
        while (isRunning && t > 0)
        {
            yield return new WaitForSeconds(interv);
            for (int i = 1; i <= amountOfBullets; i++)
            {
                float ang = (angle - 0.5f * rangeInDegrees - i * rangeInDegrees / (float) amountOfBullets) * Mathf.Deg2Rad;
                Vector3 norm = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0);
                GameObject o = Instantiate(bullet, myPosition, Quaternion.identity) as GameObject;
                o.SendMessage("InitializeBullet", new BulletInitial(bS * norm, bA * norm, bJ * norm));
            }

            bS += deltaS*interv;
            bA += deltaA*interv;
            bJ += deltaJ * interv;

            angle += rotationS * interv;
            rotationS += rotationA * interv;
            rotationA += rotationJerk * interv;
            interv = Mathf.Clamp(interv + deltaInterval, 0.01f, Mathf.Infinity);
            t -= interv;
            Debug.Log(t);
        }
        yield return null;
    }

}

[System.Serializable]
public class BulletPatternList
{
    public BulletPattern[] bulletHolder;
    public int Length()
    {
        return bulletHolder.Length;
    }
        
}

