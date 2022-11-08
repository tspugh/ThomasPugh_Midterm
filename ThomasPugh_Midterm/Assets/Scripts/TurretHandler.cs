using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHandler : MonoBehaviour
{
    public GameObject[] potentialTurrets;
    private List<GameObject> turrets;
    private int turretNum;

    private void Start()
    {
        turretNum = 0;
        turrets = new List<GameObject>();
    }

    public void AddTurret()
    {
        if(turretNum<potentialTurrets.Length)
        {
            GameObject newTurret = Instantiate(potentialTurrets[turretNum]);
            newTurret.transform.parent = this.gameObject.transform;
            turrets.Add(newTurret);
            for(int i = 0; i<turrets.Count; i++)
            {
                turrets[i].GetComponent<TurretScript>().offset = i * Mathf.PI * 2 / turrets.Count;
            }
        }
        else
        {
            turrets[turretNum % turrets.Count].GetComponent<BulletSpawner>().bulletPatterns[0].bulletHolder[0].bulletS *= 1.5f;
        }
        turretNum++;
    }
}
