using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DestructionArgs : EventArgs
{
    public GameObject destroyedGameObject;
}

public static class GameEvents
{
    public static event EventHandler<DestructionArgs> DamagableDestroyed;

    public static void InvokeDamagableDestroyed(GameObject o)
    {
        DamagableDestroyed(null, new DestructionArgs { destroyedGameObject = o });
    }
}
