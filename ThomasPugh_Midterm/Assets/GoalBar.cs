using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalBar : MonoBehaviour
{

    public GameObject bar1;
    public float bar1Value;
    public float bar1Max;
    public GameObject bar2;
    public float bar2Value;
    

    public GameObject text;

    public void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        bar1.GetComponent<RectTransform>().sizeDelta = new Vector2(rt.sizeDelta.x * Mathf.Clamp(bar1Value / bar1Max , 0f, 1f), rt.sizeDelta.y);
        bar2.GetComponent<RectTransform>().sizeDelta = new Vector2(rt.sizeDelta.x * bar2Value, rt.sizeDelta.y);
        text.GetComponent<TextMeshProUGUI>().text = bar1Value + " / " + bar1Max;
    }

}
