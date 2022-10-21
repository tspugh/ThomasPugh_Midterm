using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public GameObject[] enemies;
    public int difficulty;
}
