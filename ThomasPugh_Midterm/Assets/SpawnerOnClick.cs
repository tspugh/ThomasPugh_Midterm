using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOnClick : MonoBehaviour
{

    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
