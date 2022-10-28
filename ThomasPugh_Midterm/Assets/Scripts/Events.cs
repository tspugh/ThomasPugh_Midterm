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

public static class GameEvents
{
    public static event EventHandler<DestructionArgs> DamagableDestroyed;

    public static event EventHandler<NewGameArgs> NewGameBegin;

    public static event EventHandler<SoundArgs> ChangeSoundtrack;

    public static void InvokeDamagableDestroyed(GameObject o)
    {
        DamagableDestroyed(null, new DestructionArgs { destroyedGameObject = o });
    }

    public static void InvokeNewGameBegin(float enteredDelay)
    {
        NewGameBegin(null, new NewGameArgs { delay = enteredDelay });
    }

    public static void InvokeChangeSoundtrack(SoundtrackType s)
    {
        ChangeSoundtrack(null, new SoundArgs { newSoundtrack = s });
    }
}
