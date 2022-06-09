using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Manager : MonoBehaviour
{
    //Save system
    public static Save_Manager i { get; set; }

    private void Awake()
    {
        if (i != null)
        {
            Destroy(gameObject);
        }
        else
        {
            i = this;
        }
    }

    [System.Serializable]
    public class Save_Data
    {
        public float volumeMusic;
        public float volumeSFX;
        public int levelID;
    }

    public void Save()
    {
        Save_Data data = new Save_Data();
        data.volumeMusic = Audio_Manager.i.volumeMusic;
        data.volumeSFX = Audio_Manager.i.volumeSFX;
        data.levelID = Levels_Manager.i.levelID;
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("save", json);


    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            string json = PlayerPrefs.GetString("save");
            Save_Data data = JsonUtility.FromJson<Save_Data>(json);

            Audio_Manager.i.volumeMusic = data.volumeMusic;
            Audio_Manager.i.volumeSFX = data.volumeSFX;
            Levels_Manager.i.levelID = data.levelID;
        } else
        {
            Debug.Log("No save data found");
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey("save");
    }
        
}
