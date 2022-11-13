using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class LaserScript : BulletBehaviour
{

    public GameObject laserEnd;
    public GameObject laserObject;
    public AudioClip warningSound;
    public AudioClip spawnSound;
    public float countdown;
    public float duration;

    private LaserEnd[] ends;
    private LineRenderer lineR;
    private bool hasStarted = false;

    // Start is called before the first frame update
    new void Start()
    {
        ends = new LaserEnd[2];
        ends[0] = Instantiate(laserEnd, transform.position, Quaternion.identity).GetComponent<LaserEnd>();
        ends[1] = Instantiate(laserEnd, transform.position, Quaternion.identity).GetComponent<LaserEnd>();

        ends[0].velocity = this.velocity;
        ends[1].velocity = -1 * this.velocity;

        lineR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    new void Update()
    {
        if(!hasStarted && ends[0].hitBounds && ends[1].hitBounds)
        {
            hasStarted = true;
            StartCoroutine(DoWarningAndDie());
        }
    }

    public IEnumerator DoWarningAndDie()
    {
        lineR.SetPosition(0, ends[0].transform.position);
        lineR.SetPosition(1, ends[1].transform.position);
        float t = 0;
        float[] warningTimes = new float[9];
        for(int i = 0; i<warningTimes.Length; i++)
        {
            warningTimes[i] = (i + 1f) / (i + 2f) * countdown;
        }
        int start = 0;
        while(t<countdown)
        {
            for(int i = start; i<warningTimes.Length; i++)
            {
                if (t> warningTimes[i])
                {
                    AudioSource.PlayClipAtPoint(warningSound, Camera.main.transform.position + new Vector3(0f, 0f, 1f), 0.100f);
                    warningTimes[i] = countdown;
                    start++;
                    break;
                }
            }

            Color c = new Color(255, 0, 0, t / countdown);
            lineR.startColor = c;
            lineR.endColor = c;

            t+= Time.deltaTime;
            yield return null;
        }

        SpawnBulletsAlongPath(ends[0].transform.position, ends[1].transform.position, 1f/4f, laserObject);

        AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position + new Vector3(0f, 0f, 1f), 0.125f);

        yield return null;

        Destroy(ends[0].gameObject);
        Destroy(ends[1].gameObject);
        Destroy(gameObject);

    }

    public void SpawnBulletsAlongPath(Vector3 start, Vector3 end, float density, GameObject bullet)
    {
        Vector3 interval = (end - start);
        for(float i = 0; i < interval.magnitude; i += 1/density)
        {
            GameObject o = Instantiate(bullet, start + i * interval.normalized, Quaternion.identity);
            o.SendMessage("InitializeBullet", new BulletInitial(interval.normalized * 1f/1000f, Vector3.zero, Vector3.zero));
            o.GetComponent<BulletFloater>().bulletSurvivalTime = duration;
        }
    }
}
