using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ZoneManager : MonoBehaviour
{

    public Zone[] zones;
    public int currentZone = 0;
    public int currentWave = 0;
    public int wavesCompleted;
    public int enemiesPresent = 0;
    public float radius = 10;
    private GameObject currentBG;
    public List<GameObject> presentEnemies = new List<GameObject>();
    public GameStatus gameStatus;
    public GameObject waveText;

    public int difficulty = 0;

    protected float[] chanceToDoubleSpawn = { 0f, 0.125f };
    
    public int maxZone = 1;

    private bool pickupsSpawned;

    public List<GameObject> pickupsPresent;


    public void Awake()
    {
        GameEvents.DamagableDestroyed += OnDamagableDestroyed;
        GameEvents.EnemySpawned += OnEnemySpawned;
    }
    // Start is called before the first frame update
    void Start()
    {
        pickupsPresent = new List<GameObject>();
        currentZone = 0;
        currentWave = 0;
        enemiesPresent = 0;
        pickupsSpawned = false;
        presentEnemies = new List<GameObject>();
        spawnWave();
        waveText.GetComponent<TextMeshProUGUI>().text = GetZoneString();
        currentBG = Instantiate(zones[currentZone].background, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    

    public void incrementWave()
    {
        currentWave++;
        wavesCompleted++;
        
        if (currentZone < zones.Length && currentWave >= zones[currentZone].length+2)
        {
            currentWave = 0;
            DoEndOfZone();
        }

        waveText.GetComponent<TextMeshProUGUI>().text = GetZoneString();
    }

    public void spawnWave()
    {
        
        Wave cur = zones[currentZone].GetNextWave(currentWave);
        Debug.Log(this.name +":Wave Name: " + cur.name);
        for(int i = 0; i<cur.enemies.Length; i++)
        {
            GameObject o = cur.enemies[i];
            bool gettingNextWave = true;
            Vector3 newPos = Vector3.zero;
            float rad = radius;

            while(gettingNextWave)
            {
                newPos = new Vector3(Random.Range(gameStatus.minX, gameStatus.maxX), Random.Range(gameStatus.minY, gameStatus.maxY), 0);
                gettingNextWave = false;
                GameObject[] gobjects = SceneManager.GetActiveScene().GetRootGameObjects();
                foreach(GameObject objectthing in gobjects)
                {
                    if(objectthing.gameObject.CompareTag("Player"))
                    {
                        if ((objectthing.transform.position - newPos).magnitude <= rad)
                            gettingNextWave = true;
                    }
                }
                rad -= 0.01f;
                if (rad <= 5.0f)
                {
                    gettingNextWave = false;
                    newPos = Vector3.zero;
                    Debug.Log("Error Spawning Enemies");
                }
            }

            GameObject holder = Instantiate(o, newPos, Quaternion.identity);
            SpriteRenderer s = holder.GetComponent<SpriteRenderer>();
            if(s)
            {
                s.material = zones[currentZone].material;
            }
            EnemyBehaviour enemyBehaviour = holder.GetComponent<EnemyBehaviour>();
            if(enemyBehaviour)
            {
                enemyBehaviour.difficulty = this.difficulty;
            }
            
            presentEnemies.Add(holder);
            enemiesPresent++;


            if (Random.Range(0f, 1f) < chanceToDoubleSpawn[difficulty] && currentWave! > zones[currentZone].length)
                i--;
        }
    }

    public string GetZoneString()
    {
        if (currentZone < zones.Length)
            return "Wave " + (currentWave + 1) + " of " + (zones[currentZone].length + 2)+"\nZone "+(currentZone+1)+" of "+ (Mathf.Min(maxZone,zones.Length));
        else
            return "";
    }

    public void DoEndOfZone()
    {
        GameEvents.InvokeChangeSoundtrack(SoundtrackType.Main);
        
        GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject o in holder)
        {
            if (o.CompareTag("Enemy") || o.CompareTag("Background"))
                Destroy(o);
        }

        currentZone++;
        pickupsSpawned = false;

        if (currentZone >= Mathf.Min(zones.Length, maxZone))
        {
            GameEvents.InvokeGameOver(true);
        }
        else
        {
            currentBG = Instantiate(zones[currentZone].background, new Vector3(0, 0, 0), Quaternion.identity);
        }


    }

    public List<GameObject> SpawnPickups(int amount)
    {
        List<GameObject> potential = new List<GameObject>(zones[currentZone].upgrades);
        List<GameObject> holder = new List<GameObject>();
        amount = Mathf.Min(potential.Count, amount);
        for (int i = 0; i < amount; i++)
        {
            GameObject o = potential[Random.Range(0, potential.Count)];
            holder.Add(Instantiate(o, new Vector3(-15 + 30 * i / (amount-1), 0, 0), Quaternion.identity));
            potential.Remove(o);
        }

        return holder;
    }
    

    public void OnDamagableDestroyed(object sender, DestructionArgs d)
    {
        if (d.destroyedGameObject.CompareTag("Enemy"))
        {
            enemiesPresent--;
            presentEnemies.Remove(d.destroyedGameObject);
            
        } 
        
    }

    public void Update()
    {
        if (enemiesPresent <= 0)
        {
            enemiesPresent = 0;

            if (currentZone < zones.Length && currentWave == zones[currentZone].length && !pickupsSpawned)
            {
                pickupsPresent = SpawnPickups(Random.Range(2, 4));
                pickupsSpawned = true;
            }

            else if (currentZone < zones.Length && pickupsPresent.Count <= 0)
            {
                incrementWave();
            }

            if (currentZone < Mathf.Min(zones.Length, maxZone) && pickupsPresent.Count <= 0)
            {
                spawnWave();
            }

        }

        if(pickupsPresent.Count>0)
        {
            bool destroyAll = false;
            for(int i = 0; i<pickupsPresent.Count; i++)
            {
                if (pickupsPresent[i] == null)
                {
                    destroyAll = true;
                }
            }
            if (destroyAll)
            {
                for(int i = 0; i<pickupsPresent.Count; i++)
                if (pickupsPresent[i])
                {
                    Destroy(pickupsPresent[i]);
                }
                pickupsPresent = new List<GameObject>();
            }
        }
    }

    public void OnEnemySpawned(object sender, EnemySpawnArgs e)
    {
        if (e.enemySpawned != null && !presentEnemies.Contains(e.enemySpawned))
        {
            enemiesPresent += 1;
            presentEnemies.Add(e.enemySpawned);
        }
    }
}
