using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public GameObject[] playerObjects;
    public int playerIndex = 0;

    public int difficulty = 0;
    public Texture2D[] difficultyIcons;

    public GameObject zoneManager;
    public GameObject zoneManagersRequiredTextField;
    public GameObject healthBarButThisIsKindaASketchyCall;
    public GameObject bossHealthBarSketchy;
    public int maxZone = 1;

    public static GameManagerScript myScript;

    private GameObject zoneManagerHolder;
    private GameObject playerHolder;

    public AudioClip endRoundClip;

    private void Awake()
    {
        GameEvents.NewGameBegin += OnGameBegin;
        GameEvents.GameOver += OnGameOver;
        GameEvents.DamagableDestroyed += OnDamageableDestroyed;
        GameEvents.EnemySpawned += OnEnemySpawned;
        myScript = this;

    }
    // Start is called before the first frame update

    void Start()
    {
        if(endRoundClip)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.clip = endRoundClip;
        }
    }

    void AllowInstantDeath()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject o in holder)
            {
                if (!o.CompareTag("Player"))
                    o.GetComponent<Damageable>()?.SetHealth(0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AllowInstantDeath();
    }

    void OnGameBegin(object sender, NewGameArgs e)
    {
        StartCoroutine(SpawnGame(e.delay));
        GameEvents.InvokeChangeSoundtrack(SoundtrackType.Main);
    }

    void OnDamageableDestroyed(object sender, DestructionArgs e)
    {
        if(e.destroyedGameObject.CompareTag("Player"))
        {
            StartCoroutine(EndRound());
        }
    }

    private IEnumerator SpawnGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerHolder = Instantiate(playerObjects[playerIndex], Vector3.zero, Quaternion.identity);
        healthBarButThisIsKindaASketchyCall.GetComponent<HealthBar>().monitered = playerHolder;
        zoneManagerHolder = Instantiate(zoneManager);
        zoneManagerHolder.GetComponent<ZoneManager>().waveText = zoneManagersRequiredTextField;
        zoneManagerHolder.GetComponent<ZoneManager>().maxZone = this.maxZone;
        zoneManagerHolder.GetComponent<ZoneManager>().difficulty = this.difficulty;
        yield return null;
    }

    public IEnumerator EndRound()
    {
        yield return new WaitForEndOfFrame();
        
        GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
        GameEvents.InvokeGameOver(false);
        foreach(GameObject o in holder)
        {
            if(o.CompareTag("Enemy") || o.CompareTag("Background") || o.CompareTag("Player") || o.CompareTag("Pickup"))
                Destroy(o);
        }
        Destroy(zoneManagerHolder);
        GameEvents.InvokeChangeSoundtrack(SoundtrackType.Menu);
        yield return null;
    }

    public void SetBoss(GameObject o)
    {
        bossHealthBarSketchy.GetComponent<HealthBar>().monitered = o;
    }

    public void OnEnemySpawned(object sender, EnemySpawnArgs e)
    {
        if (e.enemySpawned.GetComponent<EnemyBehaviour>().isBoss)
            SetBoss(e.enemySpawned);
    }

    public void OnGameOver(object sender, GameOverArgs e)
    {
        if(e.won)
            gameObject.GetComponent<AudioSource>().Play();
    }

    public void IncreasePlayerIndex()
    {
        playerIndex = (playerIndex + 1) % playerObjects.Length;
    }

    public void IncreaseDifficulty()
    {
        difficulty = (difficulty + 1) % difficultyIcons.Length;
    }

}
