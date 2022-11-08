using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public string enemyName;

    public float velocity;
    public float maxVelocity;

    public float acceleration;
    public float maxAcceleration;

    public Vector3 velocityDir;
    public Vector3 accelerationDir;
    public GameStatus gameStatus;

    public bool isBoss = false;


    public virtual void Translate()
    { }

    public virtual void Rotate()
    { }


    // Update is called once per frame
    void Update()
    {
        Translate();
        Rotate();
    }
    
}
