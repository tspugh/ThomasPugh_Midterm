using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTranslateinBounds : MonoBehaviour
{
    private Vector3 target = new Vector3(0, 0, 0);
    private float xMin, xMax, yMin, yMax;
    private float speed;

    public float maxSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        SetBounds(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)), Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)));
        GenNewTarget();
        speed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(!MoveAndTest())
        {
            speed = maxSpeed;
            GenNewTarget();
        }
    }

    void SetBounds(Vector3 min, Vector3 max)
    {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        float wid = max.x - min.x;
        float hei = max.y - min.y;
        xMin = min.x + wid - r.bounds.size.x / 2f;
        xMax = max.x - wid + r.bounds.size.x / 2f;
        yMin = min.y + hei - r.bounds.size.y / 2f;
        yMax = max.y - hei + r.bounds.size.y / 2f;
    }

    void GenNewTarget()
    {
        target = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0);
    }

    bool MoveAndTest()
    {
        Vector3 motion = target - transform.position;
        transform.position += motion/motion.magnitude * speed * Time.deltaTime;
        if (motion.magnitude / speed < 0.8f) { speed -= 1 * Time.deltaTime; }
        return (motion.magnitude / speed > 0.01f);
    }
}
