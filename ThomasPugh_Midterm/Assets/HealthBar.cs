using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public GameObject monitered;

    public void Update()
    {
        slider.value = monitered.GetComponent<Damageable>().health;
        slider.maxValue = monitered.GetComponent<Damageable>().maxHealth;
    }
}
