using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject howToPlayMenu;

    EventSystem m_EventSystem;

    private void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    public void SettingsMenu()
    {
        howToPlayMenu.SetActive(false);
        settingsMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);
    }

    public void HowToPlayMenu()
    {
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);        
    }
}
