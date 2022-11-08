using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    public int maxAmountGoals = 2;

    public static Goal[] availibleGoals;

    public GameObject scoreSaveObject;
    private SaveScore saveScore;

    public GameObject[] listeningObjects;

    //the active goals, and the completed goals.
    public int[] activeGoals;
    public List<int> completedGoalIndex;

    //keeps track of the game's stats when a game is running.
    public List<GameObject> enemiesKilled;
    public int wavesCompleted;
    public int zonesCompleted;
    public float timeSinceBeginning;
    public float timeToDefeatBoss;
    public bool wasPlayerHit;
    public bool wasPickupTaken;

    // Start is called before the first frame update
    void Start()
    {
        activeGoals = new int[maxAmountGoals];

        saveScore = scoreSaveObject.GetComponent<SaveScore>();

    }

    // Update is called once per frame
    void Update()
    {

        saveScore.goalsComplete = UpdateCompletedGoalIndex(saveScore.goalsComplete);
 
    }

    List<int> UpdateCompletedGoalIndex(List<int> goalsCompletedInSave)
    {

        goalsCompletedInSave = new List<int>(goalsCompletedInSave);

        //check if any goals were met from the updated game status.
        for(int i = 0; i < availibleGoals.Length; i++)
        {

        }


        //check if the goals were met from the old saveScoreList.

        //update the completed goals in the list of goals

        return goalsCompletedInSave;
    }


    void OnGoalComplete()
    {

    }

    void NotifyUIOfCompletedGoal()
    {

    }
}
