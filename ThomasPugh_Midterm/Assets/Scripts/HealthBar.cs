using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public GameObject monitered;
    public GameObject text;
    public GameObject title;

    public void Update()
    {
        SetInvisible(false);
        if (monitered)
        {
            slider.value = monitered.GetComponent<Damageable>().health;
            slider.maxValue = monitered.GetComponent<Damageable>().maxHealth;
            text.GetComponent<TextMeshProUGUI>().text = slider.value.ToString() + "/" + slider.maxValue.ToString();
            if(title && monitered.CompareTag("Enemy"))
            {
                title.GetComponent<TextMeshProUGUI>().text = monitered.GetComponent<EnemyBehaviour>().enemyName;
            }
        }
        else
        {
            SetInvisible(true);
        }
    }

    public void SetInvisible(bool setter)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(!setter);
        }
    }

    public void SetSliderMax(int max)
    {
        slider.maxValue = max;
    }

    public void SetSliderValue(int value)
    {
        slider.value = value;
    }
}
