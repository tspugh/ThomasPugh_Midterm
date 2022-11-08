using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructionArgs : EventArgs
{
    public GameObject destroyedGameObject;
}

public class NewGameArgs : EventArgs
{
    public int difficulty;
    public float delay;
}

public class SoundArgs : EventArgs
{
    public SoundtrackType newSoundtrack;
}

public class ScoreArgs : EventArgs
{
    public int score;
    public bool resetScore;
}

public class EnemySpawnArgs : EventArgs
{
    public GameObject enemySpawned;
}


public static class GameEvents
{
    public static event EventHandler<DestructionArgs> DamagableDestroyed;

    public static event EventHandler<NewGameArgs> NewGameBegin;

    public static event EventHandler<SoundArgs> ChangeSoundtrack;

    public static event EventHandler<ScoreArgs> IncreaseScore;

    public static event EventHandler<EnemySpawnArgs> EnemySpawned;

    public static void InvokeDamagableDestroyed(GameObject o)
    {
        if(DamagableDestroyed!=null)
            DamagableDestroyed(null, new DestructionArgs { destroyedGameObject = o });
    }

    public static void InvokeNewGameBegin(float enteredDelay)
    {
        if(NewGameBegin!=null)
            NewGameBegin(null, new NewGameArgs { delay = enteredDelay });
    }

    public static void InvokeChangeSoundtrack(SoundtrackType s)
    {
        if(ChangeSoundtrack != null)
            ChangeSoundtrack(null, new SoundArgs { newSoundtrack = s });
    }

    public static void InvokeIncreaseScore(int scoreIncrease, bool reset)
    {
        if(IncreaseScore!=null)
            IncreaseScore(null, new ScoreArgs { score = scoreIncrease, resetScore = reset });
    }

    public static void InvokeEnemySpawned(GameObject ene)
    {
        if(EnemySpawned != null)
        {
            EnemySpawned(null, new EnemySpawnArgs { enemySpawned = ene });
        }
    }
}
