using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public bool isLoaded = false;
    public PlayerData playerData;
    public const string PLAYER_DATA = "PLAYER_DATA";


    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }

    public void SaveData()
    {
        if (!isLoaded) return;
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
        Debug.Log("SAVE DATA");
    }
    public void LoadData(bool isShowAOA = false)
    {
        string d = PlayerPrefs.GetString(PLAYER_DATA, "");
        if (d != "")
        {
            playerData = JsonUtility.FromJson<PlayerData>(d);
        }
        else
        {
            playerData = new PlayerData();
        }
        isLoaded = true;
    }
}
[System.Serializable]
public class PlayerData
{
    public List<int> idMerge;
    public PlayerData()
    {
        idMerge = new List<int>();
        idMerge.Add(0);
        idMerge.Add(1);
        idMerge.Add(2);
        idMerge.Add(3);
        idMerge.Add(4);
        idMerge[0] = 2;
        idMerge[1] = 1;
        idMerge[2] = 2;
        idMerge[3] = 1;
        idMerge[4] = 2;
    }
}


