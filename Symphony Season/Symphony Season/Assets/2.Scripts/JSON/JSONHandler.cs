using UnityEngine;
using Newtonsoft.Json;
// using System.Text.Json.Serialization;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.IO;
using System;
using UnityEditor.UI;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using System.Linq;

public class SSeasonJSONData
{
    public int LevelIndex { get; set; }
    /// <summary>
    /// This is a dictionary so we can store scores per level.
    /// Key: Level name.
    /// Value: SSeasonJSONLevelData instance.
    /// </summary>
    public Dictionary<string, SSeasonJSONLevelData> ScoresPerLevel { get; set; }
}

public class SSeasonJSONLevelData
{
    public SSeasonJSONScoreData HighScore { get; set; }
    // List of scores for this level, max of 5.
    public List<SSeasonJSONScoreData> Scores { get; set; }
    // Gets updated every time the player stores their last score. Loops back to 0 if > 5.
    public int LastSavedScoreIndex { get; set; }
}

public class SSeasonJSONScoreData
{
    public DateTime Date { get; set; }
    public int Score { get; set; }
}

public class JSONHandler : MonoBehaviour
{
    public SSeasonJSONData JsonData { get; set; }
    public static JSONHandler Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SSeasonJSONData GetJSONData()
    {
        string path = Application.persistentDataPath + "/sseason.json";
        JsonData = new SSeasonJSONData();

        if (File.Exists(path))
        {
            string output;
            using (StreamReader sr = new StreamReader(path))
            {
                output = sr.ReadToEnd();
            }

            JsonData = JsonConvert.DeserializeObject<SSeasonJSONData>(output);
        }

        return JsonData;
    }

    public void AddScore(string levelName, DateTime date, int score)
    {
        SSeasonJSONLevelData lvlScore;
        if (!JsonData.ScoresPerLevel.TryGetValue(levelName, out lvlScore))
        {
            lvlScore = new SSeasonJSONLevelData();
        }

        lvlScore.LastSavedScoreIndex += 1;
        if (lvlScore.LastSavedScoreIndex > 5)
        {
            lvlScore.LastSavedScoreIndex = 0;
        }
    }

    public void WriteJSON(SSeasonJSONData data)
    {
        string path = Application.persistentDataPath + "/sseason.json";
        string output = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, output);
    }
}
