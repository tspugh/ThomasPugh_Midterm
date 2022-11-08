using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{

    private float time;
    public float rotationPeriod = 5f;
    public float radius;
    public float offset;

    public float bulletSpeed;

    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        this.transform.position = transform.parent.transform.position + new Vector3(Mathf.Cos(time * Mathf.PI / rotationPeriod + offset), Mathf.Sin(time * Mathf.PI / rotationPeriod + offset), 0)*radius;
    }

    public Vector3 GetDirectionToFire()
    {
        return -(this.transform.position) + GetComponent<NearestEnemy>().GetNearestEnemy();
    }
}
