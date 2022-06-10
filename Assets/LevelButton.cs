using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelID = 0;

    private void Start()
    {
        if (levelID == 0)
        {
            FindObjectOfType<Titlescreen_Manager>().firstButtons.Add(gameObject);
        }
        GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText((levelID + 1).ToString());
        if (levelID > Levels_Manager.i.unlockedLevelID) {
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnClick(){
        FindObjectOfType<Titlescreen_Manager>().ContinueGame(levelID);
    }
}
