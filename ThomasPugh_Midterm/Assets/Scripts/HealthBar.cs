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

    public void Update()
    {
        if (monitered)
        {
            SetInvisible(false);
            slider.value = monitered.GetComponent<Damageable>().health;
            slider.maxValue = monitered.GetComponent<Damageable>().maxHealth;
            text.GetComponent<TextMeshProUGUI>().text = slider.value.ToString() + "/" + slider.maxValue.ToString();
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
}
