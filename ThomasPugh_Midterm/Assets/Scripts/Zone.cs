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
    public Color color;

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
        int maxDifficulty = Mathf.CeilToInt(GetMaxDifficulty()*(value/length));
        if (value <= length / 2 - 1 || (value > length / 2 && value < length+1))
        {
            //messy warning
            if(value > length / 2)
                GameEvents.InvokeChangeSoundtrack(SoundtrackType.Transition);

            List<Wave> temp = new List<Wave>(potentialWaves);
            while (temp.Count > 0)
            {
                int ind = Random.Range(0, temp.Count);
                if (temp[ind].difficulty <= maxDifficulty)
                {
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
