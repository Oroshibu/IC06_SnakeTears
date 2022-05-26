using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LDtkUnity;

public class Stoneable_Behavior : MonoBehaviour, ILDtkImportedFields
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

    public void OnLDtkImportFields(LDtkFields fields)
    {
        if (fields.GetBool("flip"))
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (fields.GetBool("isStone"))
        {
            mainState.SetActive(false);
            stoneStates[stoneState].SetActive(true);
        }
        else
        {
            mainState.SetActive(true);
            stoneStates[stoneState].SetActive(false);
        }

        fields.TryGetBool("idiot", out bool idiot);
        if (idiot)
        {
            mainState.GetComponent<PatrolAI>().idiot = true;
        }
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

        StoneComponent stoneComponent = stoneStates[stoneState].GetComponent<StoneComponent>();

        stoneComponent.SnapToClosestX();
        //stoneComponent.sprite.transform.rotation = Quaternion.Euler(0, 0, 20 * Mathf.Sign(stoneComponent.sprite.transform.lossyScale.x));
        
        //dotween rotation sequence
        Sequence rotaSeq = DOTween.Sequence();
        rotaSeq.Insert(0, stoneComponent.sprite.transform.DOLocalMoveY(.1f, .25f).SetEase(Ease.OutBounce));
        rotaSeq.Insert(0, stoneComponent.sprite.transform.DOLocalRotate(new Vector3(0, 0, 20), .25f).SetEase(Ease.OutBounce));
        //rotaSeq.Insert(.75f, stoneComponent.sprite.transform.DOLocalMoveY(.35f, .2f).SetEase(Ease.OutBack));
        //rotaSeq.Insert(.75f, stoneComponent.sprite.transform.DOLocalRotate(new Vector3(0, 0, 30), .2f).SetEase(Ease.OutBack));
        rotaSeq.Insert(.45f, stoneComponent.sprite.transform.DOLocalMoveY(.2f, .1f).SetEase(Ease.OutSine));
        rotaSeq.Insert(.6f, stoneComponent.sprite.transform.DOLocalMoveY(0f, .1f).SetEase(Ease.InExpo));
        //rotaSeq.Insert(.75f, stoneComponent.sprite.transform.DOLocalRotate(new Vector3(0, 0, 0), .25f).SetEase(Ease.InBack));
        rotaSeq.Insert(.4f, stoneComponent.sprite.transform.DOLocalRotate(new Vector3(0, 0, 30), .1f).SetEase(Ease.InOutSine));
        rotaSeq.Insert(.6f, stoneComponent.sprite.transform.DOLocalRotate(new Vector3(0, 0, 0), .5f).SetEase(Ease.OutBounce));
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
