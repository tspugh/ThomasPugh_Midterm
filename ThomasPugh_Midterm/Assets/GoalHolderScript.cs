using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalHolderScript : MonoBehaviour
{

    public Vector3[] goalInfo;
    public string goalString;

    public bool isDisplaying = false;
    public bool won = false;

    public GameObject textElement;

    public GameObject bar1;
    private List<GameObject> bars;

    private float firstOffset = -20f;

    
    // Update is called once per frame
    void Update()
    {
        if(isDisplaying)
        {
            textElement.GetComponent<TextMeshProUGUI>().text = goalString;
            int count = 0;
            for(int i = 0; i<Mathf.Min(bars.Count, goalInfo.Length); i++)
            {
                GameObject o = bars[i];
                GoalBar gb = o.GetComponent<GoalBar>();
                gb.bar1Value = goalInfo[i].x;
                gb.bar1Max =  goalInfo[i].y;
                if (goalInfo[i].x / goalInfo[i].y >= 1)
                    count++;
                gb.bar2Value = Mathf.Max(0f, goalInfo[i].z);
                if (goalInfo[i].z > -1f && goalInfo[i].z <= 0)
                    isDisplaying = false;
            }
            if(count >= goalInfo.Length)
            {
                isDisplaying = false;
                won = true;
            }
        }
        else if (!isDisplaying && !won)
        {
            textElement.GetComponent<TextMeshProUGUI>().text = "Incomplete";
            foreach (Transform child in transform)
            {
                if(!child.gameObject.Equals(textElement))
                    child.gameObject.SetActive(false);   
            }
        }
        else
        {
            textElement.GetComponent<TextMeshProUGUI>().text = "Complete";
        }

        transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetGoalInfo(Vector3[] goals, string text, bool reset)
    {
        goalInfo = goals;
        goalString = text;
        if (reset)
        {
            ResetBars();
            isDisplaying = true;
            won = false;
        }
        
    }

    void ResetBars()
    {
        if (bars!=null)
        {
            foreach (GameObject o in bars)
            {
                Destroy(o);
            }
        }
        bars = new List<GameObject>();
        for(int i = 0; i<goalInfo.Length; i++)
        {
            GameObject o = Instantiate(bar1) as GameObject;
            o.transform.SetParent(this.transform);
            o.transform.localPosition = new Vector3(0f, firstOffset+i*8f, 0f);
            bars.Add(o);
        }
    }
}
