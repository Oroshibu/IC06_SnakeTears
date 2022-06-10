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

    private void OnEnable()
    {
        m_EventSystem = EventSystem.current;
        SettingsMenu();
    }

    public void SettingsMenu()
    {
        Audio_Manager.i.PlaySound("menu_click");
        howToPlayMenu.SetActive(false);
        settingsMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);
    }

    public void HowToPlayMenu()
    {
        Audio_Manager.i.PlaySound("menu_click");
        settingsMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);        
    }

    public void ToggleFullscreen()
    {
        Audio_Manager.i.PlaySound("menu_click");
        //Screen.fullScreen = !Screen.fullScreen;

        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            //set res
            Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.ExclusiveFullScreen);
        }
        else
        {
            //set res
            Screen.SetResolution(Display.main.systemWidth / 2, Display.main.systemHeight / 2, FullScreenMode.Windowed);
        }

    }
}
