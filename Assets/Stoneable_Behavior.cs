using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stoneable_Behavior : MonoBehaviour
{
    [SerializeField] GameObject mainState;
    [SerializeField] List<GameObject> stoneStates;
    [SerializeField] float stonedDuration;
    //[System.NonSerialized] public bool freezed = false;

    private float stonedDurationCounting;
    private bool isStone;
    //private bool isTransforming;

    [HideInInspector]
    public int stoneState = 0;

    private void Awake()
    {
        isStone = false;
        //isTransforming = false;
    }

    void FixedUpdate()
    {
        if (isStone && stonedDurationCounting != -1f)
        {

            //stoneStates[stone_state].gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Duration", stonedDurationCounting / stonedDuration);
            stonedDurationCounting -= Time.fixedDeltaTime;

            if (stonedDurationCounting < 0)
            {
                Unstone();
            }
        }
    }


    public void Stone()
    {
        if (!isStone)
        //if (!isStone && !isTransforming)
        {
            //isTransforming = true;
            StartCoroutine(transfoStoned());
        }

    }


    IEnumerator transfoStoned()
    {
        /*
        mainState.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Animator anim = mainState.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("stoned_" + stone_state.ToString());
        }


        yield return new WaitForSeconds(1f);
        */
        yield return new WaitForSeconds(0);
        stoneStates[stoneState].transform.position = mainState.transform.position;
        stoneStates[stoneState].transform.localScale = mainState.transform.localScale;

        isStone = true;
        mainState.SetActive(false);
        //mainState.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        stoneStates[stoneState].SetActive(true);
        stonedDurationCounting = stonedDuration;
    }

    public void Unstone()
    {
        if (isStone)
        {
            mainState.transform.position = stoneStates[stoneState].transform.position;

            isStone = false;
            //isTransforming = false;
            mainState.SetActive(true);
            stoneStates[stoneState].SetActive(false);
        }

    }
}
