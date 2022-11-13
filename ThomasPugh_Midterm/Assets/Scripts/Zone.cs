using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Zone")]
public class Zone : ScriptableObject
{
    //amount of waves excluding boss and miniboss
    public int length = 5;
    public Wave[] potentialWaves;
    public Wave[] potentialMiniboss;
    public Wave[] potentialBoss;
    public GameObject background;
    public GameObject[] upgrades;
    public Material material;

    private List<Wave> givenWaves;

    public int GetMaxDifficulty()
    {
        int max = 0;
        foreach(Wave w in potentialWaves)
        {
            if (w.difficulty > max) max = w.difficulty;
        }
        return max;
    }

    public Wave GetNextWave(int value)
    {
        if (givenWaves == null || value == 0)
            givenWaves = new List<Wave>();
        int maxDifficulty = Mathf.CeilToInt(GetMaxDifficulty()*((float)value/(length+1f)))+1;
        int minDifficulty = Mathf.Min(Mathf.FloorToInt((value * 2 / (length + 1))), GetMaxDifficulty()-1)+1;
        if (value <= length / 2 - 1 || (value > length / 2 && value < length+1))
        {
            //messy warning
            if(value > length / 2 && value < length / 2 + 1)
                GameEvents.InvokeChangeSoundtrack(SoundtrackType.Transition);

            List<Wave> temp = new List<Wave>(potentialWaves);
            while (temp.Count > 0)
            {
                int ind = Random.Range(0, temp.Count);
                if (temp[ind].difficulty <= maxDifficulty && temp[ind].difficulty>=minDifficulty && !givenWaves.Contains(temp[ind]))
                {
                    givenWaves.Add(temp[ind]);
                    return temp[ind];
                }
                else
                {
                    temp.RemoveAt(ind);
                }
            }

            temp = new List<Wave>(potentialWaves);
            while (temp.Count > 0)
            {
                int ind = Random.Range(0, temp.Count);
                if (temp[ind].difficulty <= maxDifficulty && temp[ind].difficulty >= minDifficulty)
                {
                    givenWaves.Add(temp[ind]);
                    return temp[ind];
                }
                else
                {
                    temp.RemoveAt(ind);
                }
            }
        }
        else if (value <= length / 2 && value > length / 2 - 1)
        {
            //messy warning
            GameEvents.InvokeChangeSoundtrack(SoundtrackType.Miniboss);

            return potentialMiniboss[Random.Range(0, potentialMiniboss.Length)];
            
        }
        else if (value == length + 1)
        {
            //messy warning
            GameEvents.InvokeChangeSoundtrack(SoundtrackType.Boss);

            return potentialBoss[Random.Range(0, potentialBoss.Length)];
        }
        return null;
    }

}
