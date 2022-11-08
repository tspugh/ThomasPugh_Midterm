using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoalType { defeatEnemyQuantity, defeatEnemyTimed, surviveWaves, beatZoneNoPickup, beatWavesNoHit, beatZoneNoHit, getToZoneInTime }


[CreateAssetMenu(menuName = "ScriptableObjects/Goal")]
public class Goal: ScriptableObject
{
    public GoalType[] goalType;
    public GameObject enemyType;
    public int enemyAmount;
    public int waveAmount;
    public int zoneAmount;
    public float timedInterval;

    public bool AllConditionsMet(GameObject[] enemiesDestroyed, int wavesBeat, int zonesBeat, float time, bool hit, bool pickup)
    {
        bool b = true;
        foreach (GoalType g in goalType)
        {
            b = b && IsConditionMet(g, enemiesDestroyed, wavesBeat, zonesBeat, time, hit, pickup);
        }
        return b;
    }
    
    public bool IsConditionMet(GoalType goal, GameObject[] gameObjects, int wavesBeat, int zonesBeat, float time, bool hit, bool pickup)
    {
        float checkTime = (timedInterval<0) ? Mathf.Infinity : timedInterval;

        if (goal == GoalType.defeatEnemyQuantity)
            return (AmountDestroyed(gameObjects) >= enemyAmount);
        else if (goal == GoalType.defeatEnemyTimed)
            return (AmountDestroyed(gameObjects) >= enemyAmount && time < checkTime);
        else if (goal == GoalType.surviveWaves)
            return (wavesBeat >= waveAmount);
        else if (goal == GoalType.beatZoneNoPickup)
            return (zonesBeat >= zoneAmount && !pickup);
        else if (goal == GoalType.beatWavesNoHit)
            return (wavesBeat >= waveAmount && !hit);
        else if (goal == GoalType.beatZoneNoHit)
            return (zonesBeat >= zoneAmount && !hit);
        else if (goal == GoalType.getToZoneInTime)
            return (zonesBeat >= zoneAmount && time < checkTime);
        else
            return false;
    }

    public int AmountDestroyed(GameObject[] gameObjects)
    {
        int amount = 0;
        foreach (GameObject o in gameObjects)
        {
            if (o.GetComponent<EnemyBehaviour>().name == enemyType.GetComponent<EnemyBehaviour>().name)
                amount++;
        }
        return amount;
    }
}
