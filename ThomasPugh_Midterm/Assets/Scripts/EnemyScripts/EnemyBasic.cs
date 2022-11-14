using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum edgeCase {bounce, teleport};

public class EnemyBasic : EnemyBehaviour
{
    public edgeCase ec;
    public float teleportOffset = 7;
    public float rotateSpeed;

    public bool spawnBulletsOnEdge;

    protected float vel;
    protected float accel;

    public bool isSnake;
    public int snakeAmount;
    public int snakeDelay;

    protected GameObject snakeHead;
    private bool givenMaterial = false;
    private GameObject snakeO;
    protected List<Vector3> headTracks;

    private bool damageableDifficulty;
    private bool bulletDifficulty;

    
    protected float[] snakeDifficultyMod = { 1, 1.5f };


    public void SetUpDifficulty()
    {
        snakeAmount = (int)((float)snakeAmount * snakeDifficultyMod[difficulty]);
        velocity *= velocityDifficultyMod[difficulty];
        maxVelocity *= velocityDifficultyMod[difficulty];
        acceleration *= accelerationDifficultyMod[difficulty];
        maxAcceleration *= accelerationDifficultyMod[difficulty];

        
    }

    public virtual void Awake()
    {
        SetUpDifficulty();
        if (isSnake && snakeAmount > 1)
        {
            snakeAmount--;
            snakeO = Instantiate(this.gameObject, transform.position, Quaternion.identity) as GameObject;
            GameEvents.InvokeEnemySpawned(snakeO.gameObject);
            
        }
    }

    public virtual void Start()
    {
        
        GameEvents.InvokeEnemySpawned(gameObject);
        SetRandomVelocity();
        vel = Random.Range(velocity, maxVelocity);
        accel = Random.Range(acceleration, maxAcceleration);
        gameObject.AddComponent<AudioSource>();

        headTracks = new List<Vector3>();

        
    }

    public override void Rotate()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public override void Translate()
    {

        if (!givenMaterial && snakeO != null && !snakeO.GetComponent<SpriteRenderer>().material.Equals(GetComponent<SpriteRenderer>().material))
        {
            snakeO.GetComponent<EnemyBasic>().snakeHead = this.gameObject;
            snakeO.GetComponent<SpriteRenderer>().material = GetComponent<SpriteRenderer>().material;
            givenMaterial = snakeO.GetComponent<SpriteRenderer>().material.Equals(GetComponent<SpriteRenderer>().material);
        }

        Damageable damageable = GetComponent<Damageable>();
        if (!damageableDifficulty && damageable)
        {
            damageable.SetDifficulty(this.difficulty);
            damageableDifficulty = true;
        }

        BulletSpawner bs = GetComponent<BulletSpawner>();
        if(!bulletDifficulty && bs!=null)
        {
            bs.SetDifficulty(this.difficulty);
            bulletDifficulty = true;
        }

        if (snakeHead == null)
        {
            transform.position += vel * velocityDir * Time.deltaTime;
            if (ec == edgeCase.bounce)
                Bounce();
            else if (ec == edgeCase.teleport)
                Teleport();
        }
        else
        {
            headTracks.Add(snakeHead.transform.position);
            if(headTracks.Count > snakeDelay)
            {
                this.transform.position = headTracks[0];
                headTracks.RemoveAt(0);
            }
        }
    }

    public void SetRandomVelocity()
    {
        vel = Random.Range(velocity, maxVelocity);
        velocityDir = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
        if (velocityDir.magnitude == 0)
            SetRandomVelocity();
        velocityDir = velocityDir / velocityDir.magnitude;
    }

    public void Bounce()
    {
        Collider2D coll = GetComponent<Collider2D>();
        float bounds = Mathf.Max(coll.bounds.extents.x / 2, coll.bounds.extents.y / 2);
        if (transform.position.x + bounds > gameStatus.maxX)
        {
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
            transform.position = new Vector3(gameStatus.maxX - bounds, transform.position.y, transform.position.z) + velocityDir*vel * 3*Time.deltaTime;
            
            if(spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.x - bounds < gameStatus.minX)
        {
            velocityDir = new Vector3(-velocityDir.x, velocityDir.y, velocityDir.z);
            transform.position = new Vector3(gameStatus.minX + bounds, transform.position.y, transform.position.z) + velocityDir * vel * 3 * Time.deltaTime;
            
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y + bounds > gameStatus.maxY)
        {
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
            transform.position = new Vector3(transform.position.x, gameStatus.maxY - bounds, transform.position.z) + velocityDir * vel * 3 * Time.deltaTime;
            
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y - bounds < gameStatus.minY)
        {
            velocityDir = new Vector3(velocityDir.x, -velocityDir.y, velocityDir.z);
            transform.position = new Vector3(transform.position.x, gameStatus.minY + bounds, transform.position.z) + velocityDir * vel * 3 * Time.deltaTime;
            
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
    }

    public void Teleport()
    {
        Collider2D coll = GetComponent<Collider2D>();
        float bounds = Mathf.Max(coll.bounds.extents.x / 2, coll.bounds.extents.y / 2);
        if (transform.position.x - bounds > gameStatus.maxX)
        {
            transform.position = new Vector3(gameStatus.minX-bounds, transform.position.y, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.x + bounds < gameStatus.minX)
        {
            transform.position = new Vector3(gameStatus.maxX+bounds, transform.position.y, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y - bounds > gameStatus.maxY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.minY- bounds, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
        if (transform.position.y + bounds < gameStatus.minY)
        {
            transform.position = new Vector3(transform.position.x, gameStatus.maxY+ bounds, transform.position.z);
            if (spawnBulletsOnEdge)
                GetComponent<BulletSpawner>().RunPatternOnce();
        }
    }

    
}
