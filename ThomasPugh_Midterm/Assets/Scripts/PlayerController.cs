using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    

    public float bulletSpeed = 50;
    public float interval = 0.2f;
    public int amountOfBullets = 4;


    public GameStatus gameStatus;

    public Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        gameStatus.UpdateBounds(0, 0);
        UpdateBulletPattern();
    }

    // Update is called once per frame
    void Update()
    {
        gameStatus.UpdateBounds(0, 0);
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse = new Vector3(Mathf.Clamp(mouse.x, gameStatus.minX, gameStatus.maxX), Mathf.Clamp(mouse.y, gameStatus.minY, gameStatus.maxY), 0);
        Vector3 diff = mouse - transform.position;

        if(diff.x!=0|| diff.y!=0)
            transform.rotation = Quaternion.AngleAxis(-90f + Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg, Vector3.forward);

        transform.position = Vector3.SmoothDamp(transform.position,mouse,ref velocity, 100/mouseSensitivity*Time.deltaTime);
    }

    public void UpdateBulletPattern()
    {
        BulletSpawner b = GetComponent<BulletSpawner>();
        b.bulletPatterns[0].bulletHolder[0].bulletS = this.bulletSpeed;
        b.bulletPatterns[0].bulletHolder[0].interval = this.interval;
        b.interval = this.interval;
        b.bulletPatterns[0].bulletHolder[0].amountOfBullets = this.amountOfBullets;
    }

    
}
