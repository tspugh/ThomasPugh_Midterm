using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalState
{
    public List<string> enemiesKilled;
    public int wavesCompleted;
    public int zonesCompleted;
    public float timeSinceBeginning;
    public float timeToDefeatBoss;
    public bool wasPlayerHit;
    public bool wasPickupTaken;

    public GoalState()
    {
        SetEnemiesKilled(new List<string>());
        zonesCompleted = 0;
        wavesCompleted = 0;
        timeSinceBeginning = 0;
        timeToDefeatBoss = Mathf.Infinity;
        wasPlayerHit = false;
        wasPickupTaken = false;
    }

    public void SetEnemiesKilled(List<string> listy)
    {
        enemiesKilled = listy;
    }
}

public class GoalManager : MonoBehaviour
{

    public int maxAmountGoals = 2;

    public Goal[] availibleGoals;

    public GameObject scoreSaveObject;
    private SaveScore saveScore;

    public GameObject[] listeningObjects;

    //the active goals, and the completed goals.
    public List<int> activeGoals;
    public List<int> completedGoalIndex;

    //keeps track of the game's stats when a game is running.
    public GoalState gS;
    private float tempBossTime;

    public bool gameIsRunning;
    public bool notDoneYet = true;

    private void Awake()
    {
        GameEvents.GameOver += OnGameOver;
        GameEvents.NewGameBegin += OnGameBegin;
        GameEvents.DamagableDestroyed += OnDamagableDestroyed;
        GameEvents.EnemySpawned += OnEnemySpawned;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        activeGoals = new List<int>();
        gameIsRunning = false;
        notDoneYet = true;
        InitializeGameVariables();


        saveScore = scoreSaveObject.GetComponent<SaveScore>();

    }

    void InitializeGameVariables()
    {
        gS = new GoalState();
        Debug.Log("Initialized Game Variables");
    }

    
    // Update is called once per frame
    void Update()
    {
        bool resetter = false;
        if (saveScore && saveScore.goalsUnlocked)
        {
            completedGoalIndex = saveScore.goalsComplete;
            activeGoals = saveScore.activeGoals;

            if (gameIsRunning)
            {
                notDoneYet = true;
                HarvestSceneData();
                completedGoalIndex.AddRange(GetNewGoalsMet());
                gS.timeSinceBeginning += Time.deltaTime;
            }
            else
            {
                if (notDoneYet)
                {
                    InitializeGameVariables();
                    UpdateActiveGoals();
                    StopAllCoroutines();
                    notDoneYet = false;
                    resetter = true;
                }
            }

            PassUIGoals(resetter);

            saveScore.goalsComplete = completedGoalIndex;
            saveScore.activeGoals = activeGoals;
        }
 
    }

    List<int> GetNewGoalsMet()
    {
        List<int> gM = new List<int>();
        for(int i = 0; i< activeGoals.Count; i++)
        {
            if (activeGoals[i]!=-1 && availibleGoals[activeGoals[i]].AllConditionsMet(gS) && !completedGoalIndex.Contains(activeGoals[i]))
            {
                Debug.Log("A goal was met");

                gM.Add(activeGoals[i]);
                GameEvents.InvokeIncreaseCurrency(availibleGoals[activeGoals[i]].currencyValue);
                GetComponent<AudioSource>().Play();
            }
            
        }
        return gM;
    }

    public void PassUIGoals(bool reset)
    {
        foreach(GameObject obj in listeningObjects)
        {
            GoalUISystem sys = obj.GetComponent<GoalUISystem>();
            List<Vector3[]> holder = new List<Vector3[]>();
            List<string> holderstring = new List<string>();
            foreach(int goalNum in activeGoals)
            {
                Goal g = availibleGoals[goalNum];
                holder.Add(g.GetTotalCompletionRatio(gS));
                holderstring.Add(g.GetString());
            }
            sys.SetGoalHolderInfo(holder, holderstring, reset);
            sys.display = saveScore.goalsUnlocked;
        }
    }

    void UpdateActiveGoals()
    {
        for (int i = 0; i < activeGoals.Count; i++)
        {
            if (completedGoalIndex.Contains(activeGoals[i]))
            {
                int j = i;
                i = -1;
                activeGoals.RemoveAt(j);
                
            }
        }

        while(activeGoals.Count < maxAmountGoals)
        {
            int newGoal = GetNewGoal();
            if (newGoal >= 0)
                activeGoals.Add(newGoal);
            else
                maxAmountGoals--;
        }
    }

    //returns a random goal from the potential goals, excluding completed goals.
    //If there are no goals left ungiven,
    //returns null.

    //NOTE TO FIX: There is a chance for it to randomly not give a goal even though there are still goals left.
    public int GetNewGoal()
    {
        int tries = 1000 * (availibleGoals.Length - completedGoalIndex.Count);
        int choice = -1;
        while (tries > 0)
        {
            choice = UnityEngine.Random.Range(0, availibleGoals.Length);
            if (!completedGoalIndex.Contains(choice)&&!activeGoals.Contains(choice))
            {
                return choice;
            }
            tries--;
        }
        return -1;
    }

    public void OnDamagableDestroyed(object sender, DestructionArgs e)
    {
        if (e.destroyedGameObject.CompareTag("Enemy"))
        {
            gS.enemiesKilled.Add(e.destroyedGameObject.GetComponent<EnemyBehaviour>().enemyName);
            for (int i = 0; i < activeGoals.Count; i++)
            {
                if (availibleGoals[activeGoals[i]].enemyType && availibleGoals[activeGoals[i]].enemyType.GetComponent<EnemyBehaviour>().enemyName.Equals(e.destroyedGameObject.GetComponent<EnemyBehaviour>().enemyName))
                {
                    
                    StopCoroutine(CountBossTime());

                }
            }
        }
        
    }

    public void OnEnemySpawned(object sender, EnemySpawnArgs e)
    {
        if(e.enemySpawned)
        {
            for(int i = 0; i < activeGoals.Count; i++)
            {
                if (e.enemySpawned.GetComponent<EnemyBehaviour>().isBoss && availibleGoals[activeGoals[i]].enemyType && availibleGoals[activeGoals[i]].enemyType.GetComponent<EnemyBehaviour>().enemyName.Equals(e.enemySpawned.GetComponent<EnemyBehaviour>().enemyName))
                {
                    gS.timeToDefeatBoss = Mathf.Infinity;
                    StartCoroutine(CountBossTime());
                }
            }
        }
    }

    public void OnGameBegin(object sender, NewGameArgs e)
    {
        gameIsRunning = true;
        gS.timeSinceBeginning = -1 * e.delay;
    }

    public void OnGameOver(object sender, GameOverArgs e)
    {
        HarvestSceneData();
        completedGoalIndex.AddRange(GetNewGoalsMet());
        if (e.won)
            saveScore.goalsUnlocked = true;
        gameIsRunning = false;
        notDoneYet = true;
        Debug.Log(e.won);
    }

    public IEnumerator CountBossTime()
    {
        tempBossTime = 0;
        while(true)
        {
            tempBossTime += Time.deltaTime;
            gS.timeToDefeatBoss = tempBossTime;
            yield return null;
        }
    }

    public void HarvestSceneData()
    {
        GameObject[] holder = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject o in holder)
        {
            if (o.CompareTag("Player"))
            {
                gS.wasPickupTaken = o.GetComponent<PickupHandler>().gotPickup;
                if (o.GetComponent<Damageable>().health < o.GetComponent<Damageable>().maxHealth)
                    gS.wasPlayerHit = true;
            }
            if (o.CompareTag("ZoneManager"))
            {
                gS.wavesCompleted = Mathf.Max(o.GetComponent<ZoneManager>().wavesCompleted, 0);
                gS.zonesCompleted = Mathf.Max(o.GetComponent<ZoneManager>().currentZone, 0);
            }
        }
    }
}
