using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 10f;

    private float minX, maxX, minY, maxY;

    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        UpdateBounds(r.bounds.size.x/4f, r.bounds.size.y/4f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse = new Vector3(Mathf.Clamp(mouse.x, minX, maxX), Mathf.Clamp(mouse.y, minY, maxY), 0);
        Vector3 diff = mouse - transform.position;

        if(diff.x!=0|| diff.y!=0)
            transform.rotation = Quaternion.AngleAxis(-90f + Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg, Vector3.forward);

        transform.position = Vector3.SmoothDamp(transform.position,mouse,ref velocity, 100/mouseSensitivity*Time.deltaTime);
    }

    void UpdateBounds(float xOffset, float yOffset)
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xOffset;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xOffset;
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yOffset;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yOffset;
    }
}
