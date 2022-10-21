using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum edgeCase {bounce, teleport};

public class EnemyBasic : EnemyBehaviour
{
    public edgeCase ec;
    public float teleportOffset = 3;
    public float rotateSpeed;

    public void Start()
    {
        SetRandomVelocity();
    }

    public override void Rotate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public override void Translate()
    {
        transform.position += velocity * velocityDir * Time.deltaTime;
        if (ec == edgeCase.bounce)
            Bounce();
        else if (ec == edgeCase.teleport)
            Teleport();
    }

    public void SetRandomVelocity()
    {
        velocityDir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
        if (velocityDir.magnitude == 0)
            SetRandomVelocity();
        velocityDir = velocityDir / velocityDir.magnitude;
    }

    public void Bounce()
    {
        if(transform.position.x > gameStatus.maxX)
        {
            transform.position = new Vector3(gameStatus.maxX, transform.position.y, transform.position.z);
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
        }
        if (transform.position.x < gameStatus.minX)
        {
            transform.position = new Vector3(gameStatus.minX, transform.position.y, transform.position.z);
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
        }
        if (transform.position.y > gameStatus.maxY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.maxY, transform.position.z);
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
        }
        if (transform.position.y < gameStatus.minY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.minY, transform.position.z);
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
        }
    }

    public void Teleport()
    {
        if (transform.position.x > gameStatus.maxX+teleportOffset)
        {
            transform.position = new Vector3(gameStatus.minX-teleportOffset, transform.position.y, transform.position.z);
        }
        if (transform.position.x < gameStatus.minX-teleportOffset)
        {
            transform.position = new Vector3(gameStatus.maxX+teleportOffset, transform.position.y, transform.position.z);
        }
        if (transform.position.y > gameStatus.maxY+teleportOffset)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.minY-teleportOffset, transform.position.z);
        }
        if (transform.position.y < gameStatus.minY-teleportOffset)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.maxY+teleportOffset, transform.position.z);
        }
    }
}
