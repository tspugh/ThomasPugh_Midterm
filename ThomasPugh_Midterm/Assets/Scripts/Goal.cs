using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum GoalType { defeatEnemyQuantity, defeatEnemyTimed, surviveWaves, beatZoneNoPickup, beatWavesNoHit, beatZoneNoHit, getToZoneInTime, defeatBossInTime }


[CreateAssetMenu(menuName = "ScriptableObjects/Goal")]
public class Goal : ScriptableObject
{
    public GoalType[] goalType;
    public GameObject enemyType;
    public int enemyAmount;
    public int waveAmount;
    public int zoneAmount;
    public float timedInterval;

    public int currencyValue;

    public bool AllConditionsMet(GoalState gS)
    {
        bool b = true;
        for (int i = 0; i < goalType.Length; i++)
        {
            b = b && IsConditionMet(i, gS);
        }
        return b;
    }

    public bool IsConditionMet(int index, GoalState gS)
    {
        GoalType goal = goalType[index];

        float checkTime = (timedInterval < 0) ? Mathf.Infinity : timedInterval;

        if (goal == GoalType.defeatEnemyQuantity)
            return (AmountDestroyed(gS.enemiesKilled.ToArray()) >= enemyAmount);
        else if (goal == GoalType.defeatEnemyTimed)
            return (AmountDestroyed(gS.enemiesKilled.ToArray()) >= enemyAmount && gS.timeSinceBeginning < checkTime);
        else if (goal == GoalType.surviveWaves)
            return (gS.wavesCompleted >= waveAmount);
        else if (goal == GoalType.beatZoneNoPickup)
            return (gS.zonesCompleted >= zoneAmount && !gS.wasPickupTaken);
        else if (goal == GoalType.beatWavesNoHit)
            return (gS.wavesCompleted >= waveAmount && !gS.wasPlayerHit);
        else if (goal == GoalType.beatZoneNoHit)
            return (gS.zonesCompleted >= zoneAmount && !gS.wasPlayerHit);
        else if (goal == GoalType.getToZoneInTime)
            return (gS.zonesCompleted >= zoneAmount && gS.timeSinceBeginning < checkTime);
        else if (goal == GoalType.defeatBossInTime)
            return (AmountDestroyed(gS.enemiesKilled.ToArray()) >= enemyAmount && gS.timeToDefeatBoss < checkTime);
        else
            return false;
    }

    public int AmountDestroyed(string[] gameObjects)
    {
        int amount = 0;
        if (gameObjects == null||enemyType==null)
            return amount;
        foreach (string o in gameObjects)
        {
            if (o.Equals(enemyType.GetComponent<EnemyBehaviour>().enemyName))
                amount++;
        }
        return amount;
    }

    public string OneString(GoalType goal)
    {
        if (goal == GoalType.defeatEnemyQuantity&&enemyType!=null)
            return "Defeat " + enemyAmount + " " + enemyType.GetComponent<EnemyBehaviour>().enemyName + "(s)";
        else if (goal == GoalType.defeatEnemyTimed&&enemyType!=null)
            return "Defeat " + enemyAmount + " " + enemyType.GetComponent<EnemyBehaviour>().enemyName + "(s) in " + timedInterval + " seconds";
        else if (goal == GoalType.surviveWaves)
            return "Survive " + waveAmount + " waves";
        else if (goal == GoalType.beatZoneNoPickup)
            return "Defeat " + zoneAmount + " zones without getting pickups";
        else if (goal == GoalType.beatWavesNoHit)
            return "Survive " + waveAmount + " waves without taking damage";
        else if (goal == GoalType.beatZoneNoHit)
            return "Defeat " + zoneAmount + " zones without taking damage";
        else if (goal == GoalType.getToZoneInTime)
            return "Defeat " + zoneAmount + " zones in " + timedInterval + " seconds";
        else if (goal == GoalType.defeatBossInTime&&enemyType!=null)
            return "Defeat " + enemyType.GetComponent<EnemyBehaviour>().enemyName + " in " + timedInterval + " seconds";
        else
            return "Hi";
    }

    public string GetString()
    {
        string basic = OneString(goalType[0]);
        for (int i = 1; i < goalType.Length; i++)
        {
            basic += " and " + OneString(goalType[i]);
        }
        return basic+".";
    }

    // x - the quantity completed
    // y - the quantity required
    // z - additional condition (-1: no additonal condition)
    public Vector3 GetCompletionRatio(GoalType goal, GoalState gS)
    {
        if (goal == GoalType.defeatEnemyQuantity)
            return new Vector3(AmountDestroyed(gS.enemiesKilled.ToArray()), enemyAmount, -1);
        else if (goal == GoalType.defeatEnemyTimed)
            return new Vector3(AmountDestroyed(gS.enemiesKilled.ToArray()), enemyAmount, Mathf.Clamp((timedInterval - gS.timeSinceBeginning) / timedInterval, 0, 1));
        else if (goal == GoalType.surviveWaves)
            return new Vector3(gS.wavesCompleted, waveAmount, -1);
        else if (goal == GoalType.beatZoneNoPickup)
            return new Vector3(gS.zonesCompleted, zoneAmount, (gS.wasPickupTaken) ? 0 : 1);
        else if (goal == GoalType.beatWavesNoHit)
            return new Vector3(gS.wavesCompleted, waveAmount, (gS.wasPlayerHit) ? 0 : 1);
        else if (goal == GoalType.beatZoneNoHit)
            return new Vector3(gS.zonesCompleted, zoneAmount, (gS.wasPlayerHit) ? 0 : 1);
        else if (goal == GoalType.getToZoneInTime)
            return new Vector3(gS.zonesCompleted, zoneAmount, Mathf.Clamp((timedInterval - gS.timeSinceBeginning) / timedInterval, 0, 1));
        else if (goal == GoalType.defeatBossInTime)
        {
            float z = Mathf.Clamp((timedInterval - gS.timeToDefeatBoss) / timedInterval,0,1);
            return new Vector3(AmountDestroyed(gS.enemiesKilled.ToArray()), enemyAmount, (gS.timeToDefeatBoss==Mathf.Infinity)? -1 : z);
        }
        else
            return new Vector3(0, 1, -1);
    }

    public Vector3[] GetTotalCompletionRatio(GoalState gS)
    {
        Vector3[] holder = new Vector3[goalType.Length];
        for (int i = 0; i < goalType.Length; i++)
        {
            holder[i] = GetCompletionRatio(goalType[i], gS);
        }
        return holder;
    }
}
