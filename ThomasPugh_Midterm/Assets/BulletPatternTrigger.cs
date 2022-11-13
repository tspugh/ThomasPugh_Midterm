using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternTrigger : MonoBehaviour
{

    public GameObject triggered;

    private float timer = 0;
    public float maxTime = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && timer<=0)
        {
            triggered.GetComponent<BulletSpawner>().RunPatternOnce();
            timer = maxTime;
        }

    }

    public void Update()
    {
        timer -= Time.deltaTime;
    }
}
