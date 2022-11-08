using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public enum PickupType {healthUpgrade, turretUpgrade, fireSpeedUpgrade }

public class PickupScript : MonoBehaviour
{

    public PickupType pickup;
    public float maxSurvivalTime;
    private float survivalTime;
    private UnityEngine.Color color;


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (GameObject o in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (o.CompareTag("Player"))
                {
                    o.GetComponent<PickupHandler>().GetPickup(pickup);
                }
            }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        survivalTime = maxSurvivalTime;
        color = GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {

        survivalTime -= Time.deltaTime;
        if (survivalTime <= 0f)
        {
            Destroy(gameObject);
        }

        UnityEngine.Color newColor = color;
        newColor.a = survivalTime / maxSurvivalTime * color.a;
        GetComponent<SpriteRenderer>().color = newColor;
    }
}
