using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save_Manager : MonoBehaviour
{
    //Save system
    private static Save_Manager _i;

    public static Save_Manager i
    {
        get
        {
            if (_i != null)
            {
                return _i;
            }
            else
            {
                return _i = new GameObject("Save_Manager").AddComponent<Save_Manager>();
            }
        }
    }

    private void Awake()
    {
        if (_i == null && _i != this)
        {
            _i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class Save_Data
    {
        public float volumeMusic;
        public float volumeSFX;
        public int unlockedLevelID;
    }

    public void Save()
    {
        Save_Data data = new Save_Data();
        data.volumeMusic = Audio_Manager.i.volumeMusic;
        data.volumeSFX = Audio_Manager.i.volumeSFX;
        data.unlockedLevelID = Levels_Manager.i.unlockedLevelID;
        
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
            Levels_Manager.i.unlockedLevelID = data.unlockedLevelID;
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
