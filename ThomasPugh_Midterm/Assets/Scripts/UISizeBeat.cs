using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISizeBeat : MonoBehaviour
{
    private float time;
    public float interval = 0.3f;
    public float multiple = 0.9f;

    // Update is called once per frame
    void Update()
    {
        if (time > interval * 2)
        {
            transform.localScale = new Vector3(1, 1, 1);
            time = 0;
        }
        else if (time > interval)
        {
            transform.localScale = new Vector3(multiple, multiple, multiple);
        }
        else transform.localScale = new Vector3(1, 1, 1);

        time += Time.deltaTime;
    }
}
