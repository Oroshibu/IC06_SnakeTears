using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    //Singletion Pattern
    private static Game_Manager _i;

    public static Game_Manager i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i == null)
        {
            _i = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector] public Player_Controller player;

    private bool playerSpawned;
    private bool cameraFocused;
    private bool dead;
    private bool win;

    private void Start()
    {
        player = FindObjectOfType<Player_Controller>();

        Audio_Manager.i.PlayMusic(1);
    }

    public void PlayerSpawned()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Controller>();

        playerSpawned = true;

        StartGameLevel();
    }

    public void CameraFocused()
    {
        cameraFocused = true;

        StartGameLevel();
    }

    public void StartGameLevel()
    {
        if (playerSpawned && cameraFocused)
        {
            StartCoroutine(Transition_Manager.i.TransitionIn(player.transform.position, exitTimes: 0));
        }
    }

    public void LockControls()
    {
        player.LockControls();
    }

    public void UnlockControls()
    {
        player.UnlockControls();
    }

    public void LockMovement()
    {
        player.LockMovement();
    }

    public void UnlockMovement()
    {
        player.UnlockMovement();
    }

    public void LockPlayer()
    {
        player.LockControls();
        player.LockMovement();
    }

    public void UnlockPlayer()
    {
        player.UnlockControls();
        player.UnlockMovement();
    }

    public void Win()
    {
        if (win) return;
        win = true;
        player.SetHasWon(true);
        StartCoroutine(WinCoroutine());
    }

    IEnumerator WinCoroutine()
    {
        LockControls();
        yield return new WaitForSeconds(1f);
        yield return Transition_Manager.i.TransitionOut(player.transform.position);
        Scene_Manager.i.NextScene();
    }

    public void Death()
    {
        if (dead) return;
        dead = true;
        player.Die();
        StartCoroutine(DeathCoroutine());
    }

    public void StonePlayer()
    {
        if (dead) return;
        dead = true;
        player.Stone();
        Camera_Manager.i.RayCameraEffect(1, .1f);
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        LockControls();
        Camera_Manager.i.Shake(2, .2f);
        Camera_Manager.i.DeathCameraEffect(1, 1);
        yield return new WaitForSeconds(1f);
        yield return Transition_Manager.i.TransitionOut(player.transform.position, 1.5f, 0);
        Scene_Manager.i.ReloadScene();
    }

    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }

    IEnumerator RestartCoroutine()
    {
        LockPlayer();
        yield return Transition_Manager.i.TransitionOut(player.transform.position, .5f, 0);
        Scene_Manager.i.ReloadScene();
    }
}
