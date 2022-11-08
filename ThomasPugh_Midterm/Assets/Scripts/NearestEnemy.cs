using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NearestEnemy : MonoBehaviour
{
    private Vector3 nearestEnemyDir;


    private void Start()
    {
        nearestEnemyDir = Vector3.positiveInfinity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetNearestEnemy()
    {
        nearestEnemyDir = new Vector3(Screen.width, Screen.height, 0);
        foreach (GameObject o in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (o.CompareTag("Enemy"))
            {
                if ((o.transform.position - transform.position).magnitude < (nearestEnemyDir - transform.position).magnitude)
                {
                    nearestEnemyDir = o.transform.position;
                }
            }
        }
        return nearestEnemyDir;
    }

}
