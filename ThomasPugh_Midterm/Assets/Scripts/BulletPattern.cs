using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.SceneManagement;

public class Trigger
{

}

[CreateAssetMenu(menuName="ScriptableObjects/BulletPattern")]
public class BulletPattern : ScriptableObject
{
    public GameObject bullet; //the bullet to be created
    public float bulletS, bulletA, bulletJ; //the bullet speed, accel, and jerk respectively
    public float deltaS, deltaA, deltaJ;
    public Vector3 myPosition;
    public float rangeInDegrees; //the spread in which bullets will be created (>=360 is full range)
    public Vector3 direction; //the vector pointing where the center of the range should go
    public int amountOfBullets; //how many bullets are created in one fire
    public int deltaAmountOfBullets;

    public float interval; // the time between each full spread of bullets
    public float deltaInterval;
    public float duration; // the duration in total of how long bullets are spawned

    public float rotationSpeed; // how fast the spawner rotates
    public float rotationAccel; // how fast the 
    public float rotationJerk;

    public bool shootAtPlayer;
    public bool continueToShootAtPlayer;

    public bool isRunning = true;

    public bool randomAngle = true;

    public float deltaPitch;

    public float angle;

    public AudioClip mySound;

    

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

