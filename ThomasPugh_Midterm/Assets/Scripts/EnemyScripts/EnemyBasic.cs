using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum edgeCase {bounce, teleport};

public class EnemyBasic : EnemyBehaviour
{
    public edgeCase ec;
    public float teleportOffset = 3;
    public float rotateSpeed;

    public bool spawnBulletsOnEdge;

    protected float vel;
    protected float accel;

    public void Start()
    {
        GameEvents.InvokeEnemySpawned(gameObject);
        SetRandomVelocity();
        vel = Random.Range(velocity, maxVelocity);
        accel = Random.Range(acceleration, maxAcceleration);
        gameObject.AddComponent<AudioSource>();
    }

    public override void Rotate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public override void Translate()
    {
        transform.position += vel * velocityDir * Time.deltaTime;
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
        Collider2D coll = GetComponent<Collider2D>();
        if(transform.position.x + coll.bounds.extents.x/2 > gameStatus.maxX)
        {
            transform.position = new Vector3(gameStatus.maxX - coll.bounds.extents.x/2, transform.position.y, transform.position.z);
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
            if(spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.x - coll.bounds.extents.x/2 < gameStatus.minX)
        {
            transform.position = new Vector3(gameStatus.minX + coll.bounds.extents.x/2, transform.position.y, transform.position.z);
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y + coll.bounds.extents.y/2 > gameStatus.maxY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.maxY - coll.bounds.extents.y/2, transform.position.z);
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y - coll.bounds.extents.y/2 < gameStatus.minY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.minY + coll.bounds.extents.y/2, transform.position.z);
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
    }

    public void Teleport()
    {
        Collider2D coll = GetComponent<Collider2D>();
        if (transform.position.x - coll.bounds.extents.x/2 > gameStatus.maxX)
        {
            transform.position = new Vector3(gameStatus.minX-coll.bounds.extents.x/2, transform.position.y, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.x + coll.bounds.extents.x/2 < gameStatus.minX)
        {
            transform.position = new Vector3(gameStatus.maxX+coll.bounds.extents.x/2, transform.position.y, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y - coll.bounds.extents.y/2 > gameStatus.maxY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.minY- coll.bounds.extents.y/2, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y + coll.bounds.extents.y/2 < gameStatus.minY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.maxY+ coll.bounds.extents.y/2, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
    }
}
