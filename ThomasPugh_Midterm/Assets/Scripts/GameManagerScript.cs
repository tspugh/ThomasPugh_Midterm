using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public GameObject playerObject;
    public GameObject zoneManager;
    public GameObject zoneManagersRequiredTextField;
    public GameObject healthBarButThisIsKindaASketchyCall;
    public GameObject bossHealthBarSketchy;
    public int maxZone = 1;

    public static GameManagerScript myScript;

    private GameObject zoneManagerHolder;
    private GameObject playerHolder;

    private void Awake()
    {
        GameEvents.NewGameBegin += OnGameBegin;
        GameEvents.DamagableDestroyed += OnDamageableDestroyed;
        GameEvents.EnemySpawned += OnEnemySpawned;
        myScript = this;

    }
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject o in holder)
            {
                if (!o.CompareTag("Player"))
                    o.GetComponent<Damageable>()?.SetHealth(0f);
            }
        }
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
            EndRound();
        }
    }

    private IEnumerator SpawnGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerHolder = Instantiate(playerObject, Vector3.zero, Quaternion.identity);
        healthBarButThisIsKindaASketchyCall.GetComponent<HealthBar>().monitered = playerHolder;
        zoneManagerHolder = Instantiate(zoneManager);
        zoneManagerHolder.GetComponent<ZoneManager>().waveText = zoneManagersRequiredTextField;
        zoneManagerHolder.GetComponent<ZoneManager>().maxZone = this.maxZone;
        yield return null;
    }

    public void EndRound()
    {
        GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach(GameObject o in holder)
        {
            if(o.CompareTag("Enemy")||o.CompareTag("Background")||o.CompareTag("Player")||o.CompareTag("Pickup"))
                Destroy(o);
        }
        Destroy(zoneManagerHolder);
        GameEvents.InvokeChangeSoundtrack(SoundtrackType.Menu);

    }

    public void TriggerPlayerDeath()
    {
        if(playerHolder)
            playerHolder.GetComponent<Damageable>().health = 0;
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

}
