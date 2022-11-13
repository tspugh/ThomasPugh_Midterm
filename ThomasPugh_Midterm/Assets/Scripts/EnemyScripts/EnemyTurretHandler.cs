using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretHandler : MonoBehaviour
{
    public GameObject[] potentialTurrets;
    public int maxAmountOfTurrets;
    public int beginningTurrets;
    public float radius;
    public float rotationPeriod;

    private float time;

    public List<GameObject> turretsPresent;

    private void Awake()
    {
        GameEvents.DamagableDestroyed += OnDamagableDestroyed;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<beginningTurrets; i++)
        {
            InstantiateNewTurret();
        }
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<turretsPresent.Count; i++)
        {
            GameObject o = turretsPresent[i];
            o.GetComponent<SpriteRenderer>().material = GetComponent<SpriteRenderer>().material;
            o.GetComponent<Damageable>().maxHealth /= 2;
            float angle = time * Mathf.PI * 2 / rotationPeriod + i * Mathf.PI * 2 / turretsPresent.Count;
            o.transform.position = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;
        }
        time += Time.deltaTime;
        if(time > (Mathf.PI * 12 / rotationPeriod))
        {
            time = 0;
            if(turretsPresent.Count < maxAmountOfTurrets)
            {
                InstantiateNewTurret();
            }
        }
        if (turretsPresent.Count <= 0)
        {
            InstantiateNewTurret();
            InstantiateNewTurret();
        }
    }

    void InstantiateNewTurret()
    {
        GameObject o = Instantiate(potentialTurrets[Random.Range(0, potentialTurrets.Length)], transform.position, Quaternion.identity) as GameObject;
        turretsPresent.Add(o);
        GameEvents.InvokeEnemySpawned(o);
    }

    void OnDamagableDestroyed(object sender, DestructionArgs e)
    {
        for(int i = 0; i<turretsPresent.Count; i++)
        {
            GameObject o = turretsPresent[i];
            if(o.gameObject.Equals(e.destroyedGameObject))
            {
                turretsPresent.Remove(o);
                i = 0;
            }
        }
    }
}
