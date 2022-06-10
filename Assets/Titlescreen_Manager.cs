using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Titlescreen_Manager : MonoBehaviour // required interface for OnSelect
{
    EventSystem m_EventSystem;
    private bool transitioningToNextScene = false;

    public List<GameObject> firstButtons;

    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject settingsMenu;


    public void StartGame()
    {
        if (transitioningToNextScene) return;
        StartCoroutine(StartGameCoroutine(0));
    }

    public void ContinueGame(int levelID = 0)
    {
        if (transitioningToNextScene) return;
        StartCoroutine(StartGameCoroutine(levelID));
    }
    
    IEnumerator StartGameCoroutine(int levelID)
    {
        transitioningToNextScene = true;
        yield return Transition_Manager.i.TransitionOut(1f,0f);
        //Scene_Manager.i.NextScene();
        Levels_Manager.i.levelID = levelID;
        Scene_Manager.i.LoadScene(2);
    }

    public void LevelsMenu()
    {
        Audio_Manager.i.PlaySound("menu_click");        
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        levelsMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);
    }

    public void SettingsMenu()
    {
        Audio_Manager.i.PlaySound("menu_click");        
        mainMenu.SetActive(false);
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);
    }
    
    public void Back()
    {
        Audio_Manager.i.PlaySound("menu_click");
        levelsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        m_EventSystem.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Audio_Manager.i.PlaySound("menu_click");
        Application.Quit();
    }

    private void Start()
    {
        m_EventSystem = EventSystem.current;
        StartCoroutine(Transition_Manager.i.TransitionIn(exitTimes: 0));
        Audio_Manager.i.PlayMusic(1);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<Vector2>() == Vector2.zero)
        {
            FocusFirstButton();
        }
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            FocusFirstButton();
        }
    }

    private void FocusFirstButton()
    {
        if (m_EventSystem.currentSelectedGameObject == null)
        {
            foreach (var button in firstButtons)
            {
                if (button.activeInHierarchy)
                {
                    m_EventSystem.SetSelectedGameObject(button);
                    break;
                }
            }
        }
    }
}
