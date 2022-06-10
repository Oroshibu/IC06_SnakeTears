using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    EventSystem m_EventSystem;    
    public Image background;
    public RectTransform pauseMenu;

    public List<GameObject> firstButtons;

    private void Start()
    {
        m_EventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        Game_Manager.i.ToggleUIControls(true);
        
        background.DOKill();
        pauseMenu.DOKill();
        Time.timeScale = 0;
        background.color = new Color(0, 0, 0, 0);
        background.DOFade(.8f, .3f).SetEase(Ease.OutCubic).SetUpdate(true);
        pauseMenu.anchoredPosition = new Vector2(0, 1500);
        pauseMenu.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Game_Manager.i.ToggleUIControls(false);        
    }

    public void Resume()
    {
        Audio_Manager.i.PlaySound("menu_click");
        background.DOKill();
        pauseMenu.DOKill();
        background.DOFade(0, .5f).SetEase(Ease.InCubic).SetUpdate(true);
        pauseMenu.DOAnchorPos(new Vector2(0, 1500), .5f).SetEase(Ease.InCubic).SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
    }

    public void Restart()
    {
        Audio_Manager.i.PlaySound("menu_click");
        Resume();
        Time.timeScale = 1;
        Game_Manager.i.Restart();
    }

    public void Exit()
    {
        Audio_Manager.i.PlaySound("menu_click");
        Resume();
        Time.timeScale = 1;
        Game_Manager.i.Exit();
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
