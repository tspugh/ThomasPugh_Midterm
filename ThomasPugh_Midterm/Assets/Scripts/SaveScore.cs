using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;
using System;

public class SaveScore : MonoBehaviour
{
    

    public bool goalsUnlocked;
    public int highScore = 0;
    public int currency = 0;
    public int maxZone = 1;
    public Goal[] goals;

    public List<int> goalsComplete;

    public int[] nextZonePrice;

    public GameObject[] highscoreObjects;
    public GameObject[] currencyObjects;
    public GameObject gameManager;
    public GameObject nextZoneUnlock;


    public string filename;

    private void Awake()
    {
        
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        SaveData save = new SaveData();
        save.highScore = highScore;
        save.currency = currency;
        save.maxZone = maxZone;
        save.goalsUnlocked = this.goalsUnlocked;
        save.goalsCompleted = this.goalsComplete.ToArray();

        bf.Serialize(file, save);
        file.Close();

    }

    public void LoadGame()
    {
        if (Exists())
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();

            this.highScore = save.highScore;
            this.currency = save.currency;
            this.maxZone = save.maxZone;

            this.goalsUnlocked = save.goalsUnlocked;
            this.goalsComplete = new List<int>(save.goalsCompleted);
        }
    }

    bool Exists()
    {
        return File.Exists(Application.persistentDataPath + "/" + filename);
    }

    private void Update()
    {
        foreach(GameObject o in highscoreObjects)
        {
            TextMeshProUGUI score = o.GetComponent<TextMeshProUGUI>();
            if (score)
                score.text = "HIGHSCORE: " + highScore;
        }

        foreach(GameObject o in currencyObjects)
        {
            TextMeshProUGUI curr = o.GetComponent<TextMeshProUGUI>();
            if (curr)
                curr.text = "CURRENCY: " + currency;
        }

        TextMeshProUGUI price = nextZoneUnlock.GetComponent<TextMeshProUGUI>();
        if (price && nextZonePrice.Length >= maxZone)
            price.text = "NEXT ZONE: " + nextZonePrice[maxZone - 1];
        else if (price)
            price.text = "NO NEXT ZONE";

        gameManager.GetComponent<GameManagerScript>().maxZone = this.maxZone;
    }

    public void UpdateHighscore(int score)
    {
        if (score>highScore)
        {
            highScore = score;
            currency += score;
        }
    }

    public void BuyNextZone()
    {
        if (nextZonePrice.Length >= maxZone && currency >= nextZonePrice[maxZone-1])
        {
            currency -= nextZonePrice[maxZone - 1];
            maxZone++;
        }
    }


    //returns a random goal from the potential goals, excluding completed goals.
    //If there are no goals left ungiven,
    //returns null.

    //NOTE TO FIX: There is a chance for it to randomly not give a goal even though there are still goals left.
    public Goal GetNewGoal()
    {
        int tries = 1000*(goals.Length-goalsComplete.Count);
        Goal goaltogive = null;
        while(tries > 0)
        {
            int choice = UnityEngine.Random.Range(0, goals.Length);
            if(!goalsComplete.Contains(choice))
            {
                tries = 0;
                goaltogive = goals[choice];
            }
            tries--;
        }
        return goaltogive;
    }

    //adds a completed goal to the completed goal list stored in this object
    public void AddGoalCompleted(Goal goal)
    {
        for(int i = 0; i<goals.Length; i++)
        {
            if (goals[i].Equals(goal))
                goalsComplete.Add(i);
        }
    }

    
}