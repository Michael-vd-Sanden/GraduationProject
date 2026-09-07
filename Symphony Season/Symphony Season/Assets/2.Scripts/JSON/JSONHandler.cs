using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using UnityEditor.UI;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using System.Linq;

public class SSeasonJSONData
{
    public bool AppHasStarted { get; set; } //if the towerworld scene has been started from the application or returned from another scene
    public int CurrentLevelIndex { get; set; }
    public List<int> LevelIndexes { get; set; } //the LvIndex ID is the list nr. The value is the level index itself
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

        //Debug.Log(Application.persistentDataPath);
    }

    public async Task CreateFirsttimeData()
    {
        SSeasonJSONData retData = new SSeasonJSONData();

        string output = @"{
                'AppHasStarted': true,
                'CurrentLevelIndex': 0,
                'LevelIndexes': [0,0]
            }";
        retData = JsonConvert.DeserializeObject<SSeasonJSONData>(output);

        JSONHandler.Instance.WriteJSON(retData);
        Debug.Log("Created data");

        await Task.Yield();
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
