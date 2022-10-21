using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneManager : MonoBehaviour
{

    public Zone[] zones;
    public int currentZone = 0;
    public int currentWave = 0;
    public int enemiesPresent = 0;
    private GameObject currentBG;
    public List<GameObject> presentEnemies = new List<GameObject>();
    public GameStatus gameStatus;
    public GameObject waveText;


    public void Awake()
    {
        GameEvents.DamagableDestroyed += OnDamagableDestroyed;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnWave();
        waveText.GetComponent<TextMeshProUGUI>().text = GetZoneString();
        currentBG = Instantiate(zones[currentZone].background, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    

    public void incrementWave()
    {
        currentWave++;
        
        if (currentWave > zones[currentZone].length+2)
        {
            currentWave = 0;
            DoEndOfZone();
        }

        waveText.GetComponent<TextMeshProUGUI>().text = GetZoneString();
    }

    public void spawnWave()
    {
        Wave cur = zones[currentZone].GetNextWave(currentWave);
        foreach(GameObject o in cur.enemies)
        {
            GameObject holder = Instantiate(o, new Vector3(Random.Range(gameStatus.minX, gameStatus.maxX), Random.Range(gameStatus.minY, gameStatus.maxY), 0), Quaternion.identity);
            SpriteRenderer s = holder.GetComponent<SpriteRenderer>();
            if(s)
            {
                s.color = zones[currentZone].color;
            }
            
            presentEnemies.Add(holder);
            enemiesPresent++;
        }
    }

    public string GetZoneString()
    {
        return "Wave " + (currentWave+1) + " of " + (zones[currentZone].length+2);
    }

    public void DoEndOfZone()
    {

    }

    public void OnDamagableDestroyed(object sender, DestructionArgs d)
    {
        if (d.destroyedGameObject.CompareTag("Enemy"))
        {
            enemiesPresent--;
        }
        if(enemiesPresent<=0)
        {
            enemiesPresent = 0;
            incrementWave();
            //Destroy(currentBG);
            spawnWave();
        }
    }

    public void Update()
    {
        Debug.Log(zones[currentZone].length);
    }
}
