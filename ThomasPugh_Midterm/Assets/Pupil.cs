using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pupil : MonoBehaviour
{

    public float radius = 0.35f;

    private GameObject watched;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse = (mouse - transform.parent.transform.position).normalized;
        transform.position = transform.parent.transform.position + mouse * radius;
    }
}
