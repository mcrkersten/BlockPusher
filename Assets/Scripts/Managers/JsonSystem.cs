using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonSystem
{
    public static string _directory = "/SaveData/";
    public static string _fileName = "SaveFile.txt";

    public static void Save(SaveFile fileToSave)
    {
        string dir = Application.persistentDataPath + _directory;

        if(!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(fileToSave);
        File.WriteAllText(dir + _fileName, json);
    }

    public static int GetUnixTime()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}

public struct SaveFile
{
    public string gameName;
    public int timePlayed;
    public int score;
    public int timeJsonMade;
}
