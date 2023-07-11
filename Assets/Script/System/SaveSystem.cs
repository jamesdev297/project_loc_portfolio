using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void SavePlayerData (PlayerModel player) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = GameManager.instance.persistentDataPath + "/" + player.uid.GetHashCode() + ".fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        
        Debug.Log("Save file!" + path);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string uid) {
        string path = GameManager.instance.persistentDataPath + "/" + uid.GetHashCode() + ".fun";
        if (File.Exists(path)) {
            Debug.Log("Load file exist " + path);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        } else {
            Debug.Log("Load file not found in " + path);
            return null;
        }
    }
}