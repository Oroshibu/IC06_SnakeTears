using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{
    public int levelsCount;
    public GameObject levelButtonPrefab;
    public Transform buttonsContainerTransform;

    private void Start()
    {
        for (int i = 0; i < levelsCount; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, buttonsContainerTransform);
            levelButton.GetComponent<LevelButton>().levelID = i;
        }
    }
}
