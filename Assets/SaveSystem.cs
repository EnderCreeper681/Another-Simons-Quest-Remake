using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Stats stats, Attacking attacking, Saving saving)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SavedData.json";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            PlayerData data = new PlayerData(stats, attacking, saving);
            formatter.Serialize(stream, data);
        }
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/SavedData.json";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }           
        }
        else 
        { 
            //Debug.LogError("No save file present in path: " + path);
            return null;
        }
    }
}
