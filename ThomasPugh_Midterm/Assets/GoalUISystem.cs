using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalUISystem : MonoBehaviour
{
    public GameObject goalHolder;

    public float yOffset = -40f;

    public List<GameObject> goalHolderList;
    public List<Vector3[]> goals;
    public List<string> texts;

    public bool display = true;

    public 
    // Start is called before the first frame update
    void Start()
    {
        SetGoalHolderInfo(new List<Vector3[]>(), new List<string>(), false);
    }

    // Update is called once per frame
    void Update()
    {
        if(goals!= null && goals.Count>0 && display)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    //only pass reset as true if the goals[][] change, not their values.
    public void SetGoalHolderInfo(List<Vector3[]> goals, List<string> texts, bool reset)
    {
        this.goals = goals;
        this.texts = texts;
        Reset(reset);
    }

    public void Reset(bool reset)
    {
        if (reset && goalHolderList.Count > 0)
        {
            foreach(GameObject o in goalHolderList)
            {
                Destroy(o);
            }
            goalHolderList = new List<GameObject>();
        }
        for(int i = 0; i<Mathf.Min(this.goals.Count,this.texts.Count); i++)
        {

            GameObject o;
            if (reset)
            {
                o = Instantiate(goalHolder);
                goalHolderList.Add(o);
            }
            else
            {
                if (i < goalHolderList.Count)
                    o = goalHolderList[i];
                else
                    break;
            }

            o.transform.SetParent(this.transform);
            o.transform.localPosition = new Vector3(0, yOffset + i * 58, 0);
            o.GetComponent<GoalHolderScript>().SetGoalInfo(goals[i], texts[i], reset);
        }
    }
}
