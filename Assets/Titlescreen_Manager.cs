using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Titlescreen_Manager : MonoBehaviour, ISelectHandler, IMoveHandler // required interface for OnSelect
{

    private bool transitioningToNextScene = false;

    public GameObject firstButton;

    public void StartGame()
    {
        if (transitioningToNextScene) return;
        StartCoroutine(StartGameCoroutine(1));
    }

    public void ContinueGame(int levelID = 1)
    {
        if (transitioningToNextScene) return;
        StartCoroutine(StartGameCoroutine(levelID));
    }

    IEnumerator StartGameCoroutine(int levelID)
    {
        transitioningToNextScene = true;
        yield return Transition_Manager.i.TransitionOut(1f);
        //Scene_Manager.i.NextScene();
        Scene_Manager.i.LoadScene(levelID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Do this when the selectable UI object is selected.
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(this.gameObject.name + " was selected");
    }

//    private void Start()
//    {
//        if (EventSystem.currentSelectedGameObject == null)
//        {
//            if (EventSystem.currentSelectedGameObject == null)
//.SetSelectedGameObject(firstButton);
//        }
//    }


    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("MUNGILOS");
        //throw new System.NotImplementedException();
    }
}
