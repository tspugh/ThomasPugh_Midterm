using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float velocity;
    public float acceleration;
    public Vector3 velocityDir;
    public Vector3 accelerationDir;
    public GameStatus gameStatus;


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
