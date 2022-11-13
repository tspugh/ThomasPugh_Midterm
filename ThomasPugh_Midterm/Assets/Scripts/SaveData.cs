using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[Serializable]
public class SaveData
{
    public int highScore;
    public int currency;
    public int maxZone;
    public int[] goalsCompleted;
    public int[] activeGoals;
    public bool goalsUnlocked;
    
}

