using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface ISavesystem
{
    void Save(SaveData saveData);
    SaveData Load();
}

[System.Serializable]

public class SaveData
{
    public Vector2 lastCheckpointPosition;
    public int score;
}

public class SaveSystem: ISavesystem
{
    private string filePath = Application.persistentDataPath + "/setting.json";
    
    public void Save(SaveData saveData)
    {   
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, JsonUtility.ToJson(saveData)); 
        Debug.Log("filepath has been saved in" + filePath);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
    }

    SaveData ISavesystem.Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json); 
            return saveData; 
        }

        else
        {      
            Debug.LogWarning("Save file not found. Returning default SaveData.");
            return new SaveData(); 
        }
    }
}
