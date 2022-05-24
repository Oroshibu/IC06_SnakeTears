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
        Transform world = Instantiate(level.transform.GetChild(0).GetChild(levelID)).transform;
        world.position = new Vector3(0, world.position.y, 0);
    }
}
        