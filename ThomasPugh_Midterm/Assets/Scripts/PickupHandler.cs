using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{

    public bool gotPickup = false;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        gotPickup = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPickup(PickupType pickup)
    {
        gotPickup = true;
        if(pickup == PickupType.healthUpgrade)
        {
            Damageable d = GetComponent<Damageable>();
            d.maxHealth++;
            d.health++;
        }
        else if(pickup == PickupType.turretUpgrade)
        {
            GetComponent<TurretHandler>().AddTurret();
        }
        else if(pickup == PickupType.fireSpeedUpgrade)
        {
            playerController.bulletSpeed *= 1.5f;
            playerController.UpdateBulletPattern();
        }
    }
}
