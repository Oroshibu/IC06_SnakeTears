using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using LDtkUnity;

public class Level_Importer : MonoBehaviour
{
    public GameObject level;
    
    private void Awake()
    {
        int levelID = Levels_Manager.i.levelID;

        //sort level children by their x position
        List<Transform> levelChildren = new List<Transform>();
        foreach (Transform child in level.transform.GetChild(0).transform)
        {
            levelChildren.Add(child);
        }
        levelChildren.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        Transform levelInstance = Instantiate(levelChildren[levelID].gameObject, transform.position, transform.rotation).transform;
        //Transform world = Instantiate(level.transform.GetChild(0).GetChild(levelID)).transform;
        levelInstance.position = new Vector3(0, levelInstance.position.y, 0);
    }
}
        