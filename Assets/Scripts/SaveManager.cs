using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Load();
        //Save();
        //Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Save()
    {
        PlayerPrefs.SetInt("bs", GameManager.battleScore);
        PlayerPrefs.SetInt("s", GameManager.score);
        PlayerPrefs.SetInt("d", GameManager.defeats);
        PlayerPrefs.SetInt("r", GameManager.retry);
        PlayerPrefs.SetInt("h", GameManager.howManyLoad);
        //PlayerPrefs.SetInt("ss", GameManager.stageScore);
        //PlayerPrefs.SetInt("sd", GameManager.stageDefeats);
        PlayerPrefs.SetInt("tbs", GameManager.totalBattleScore);
        PlayerPrefs.SetInt("ts", GameManager.totalScore);
        PlayerPrefs.SetInt("td", GameManager.totalDefeats);
        PlayerPrefs.SetInt("tr", GameManager.totalRetry);
        PlayerPrefs.SetInt("p", CheckPointManager.progress);
        PlayerPrefs.SetString("ns", GameManager.nowScene);

        PlayerPrefs.Save();
        Debug.Log("SaveDone");
    }

    public static void Load()
    {
        GameManager.battleScore = PlayerPrefs.GetInt("bs", 100);
        GameManager.score = PlayerPrefs.GetInt("s", 0);
        GameManager.defeats = PlayerPrefs.GetInt("d", 0);
        GameManager.retry = PlayerPrefs.GetInt("r", 0);
        GameManager.howManyLoad = PlayerPrefs.GetInt("h", 0);
        //GameManager.stageScore = PlayerPrefs.GetInt("", );
        //GameManager.stageDefeats = PlayerPrefs.GetInt("", );
        GameManager.totalBattleScore = PlayerPrefs.GetInt("tbs", 0);
        GameManager.totalScore = PlayerPrefs.GetInt("ts", 0);
        GameManager.totalDefeats = PlayerPrefs.GetInt("td", 0);
        GameManager.totalRetry = PlayerPrefs.GetInt("tr", 0);
        CheckPointManager.progress = PlayerPrefs.GetInt("p", 0);
        GameManager.nowScene = PlayerPrefs.GetString("ns", "");

    }

    public static void Show()
    {
        Debug.Log(GameManager.battleScore);
        Debug.Log(GameManager.score);
        Debug.Log(GameManager.defeats);
        Debug.Log(GameManager.retry);
        Debug.Log(GameManager.howManyLoad);
        Debug.Log(GameManager.totalBattleScore);
        Debug.Log(GameManager.totalScore);
        Debug.Log(GameManager.totalDefeats);
        Debug.Log(GameManager.totalRetry);
        Debug.Log(CheckPointManager.progress);
        Debug.Log(GameManager.nowScene);

    }

    public void Reset()
    {
        GameManager.battleScore = 100;
        GameManager.score = 0;
        GameManager.defeats = 0;
        GameManager.retry = 0;
        GameManager.howManyLoad = 0;
        //GameManager.stageScore = PlayerPrefs.GetInt("", );
        //GameManager.stageDefeats = PlayerPrefs.GetInt("", );
        GameManager.totalBattleScore = 0;
        GameManager.totalScore = 0;
        GameManager.totalDefeats = 0;
        GameManager.totalRetry = 0;
        CheckPointManager.progress = 0;
        GameManager.nowScene = "";

    }
}
