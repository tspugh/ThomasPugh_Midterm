using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{

    public int roundscore;

    private TextMeshProUGUI textholder;

    public GameObject highscoreManager;

    private void Awake()
    {
        GameEvents.IncreaseScore += IncreaseScore;
    }
    // Start is called before the first frame update
    void Start()
    {
        textholder = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textholder.text = "SCORE: " + (roundscore%9999999);
    }

    void IncreaseScore(object sender, ScoreArgs s)
    {
        roundscore += s.score;
        if (s.resetScore)
        {
            highscoreManager.GetComponent<SaveScore>().UpdateHighscore(roundscore);
            roundscore = 0;
        }
    }

}
