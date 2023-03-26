using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData(SaveManager.SaveInfoClass boards)
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.skm";
        FileStream stream = new FileStream(path, FileMode.Create);
        bf.Serialize(stream, boards);
        
        stream.Close();
        //Debug.Log("Save path: " + path);
    }

    public static SaveManager.SaveInfoClass LoadBoards()
    {
        string path = Application.persistentDataPath + "/player.skm";
        //Debug.Log("Load path: " + path);
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveManager.SaveInfoClass saveInfo = bf.Deserialize(stream) as SaveManager.SaveInfoClass;

            stream.Close();

            return saveInfo;
        }
        else
        {
            Debug.Log("SaveSystem: LoadPlayer: No save data found at - " +path);
            return null;
        }
    }

    public static bool FileExist()
    {
        string path = Application.persistentDataPath + "/player.skm";
        return File.Exists(path);
    }

    public static void DeleteSaveFiles()
    {
        string path = Application.persistentDataPath + "/player.skm";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
