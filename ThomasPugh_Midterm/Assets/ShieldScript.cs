using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{

    public GameObject theProtected;
    public float rotateSpeed = 30;

    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        angle = Random.Range(0f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        if(theProtected == null)
        {
            GetComponent<Damageable>().health = 0;
        }
        else
        {
            transform.position = theProtected.transform.position;
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }
}
