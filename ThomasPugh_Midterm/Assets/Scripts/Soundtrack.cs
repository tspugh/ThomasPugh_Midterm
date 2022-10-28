using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundtrackType { Intro, Menu,  Main, Miniboss, Transition, Boss, Outro }



[CreateAssetMenu(menuName = "ScriptableObjects/Soundtrack")]
public class Soundtrack : ScriptableObject
{
    public AudioClip intro, menu, main, miniboss, transition, boss;

    public AudioClip GetTrack(SoundtrackType s)
    {
        if (s == SoundtrackType.Intro)
            return intro;
        else if(s == SoundtrackType.Menu)
            return menu;
        else if (s == SoundtrackType.Miniboss)
            return miniboss;
        else if (s == SoundtrackType.Transition)
            return transition;
        else if (s == SoundtrackType.Boss)
            return boss;
        else
            return main;
    }
}