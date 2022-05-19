using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Win_Component : MonoBehaviour
{
    Animator animator;

    public SpriteRenderer grapeSprite;
    public SpriteRenderer raysSprite;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Win();
        }
    }

    private void Win()
    {
        Game_Manager.i.Win();
        StartCoroutine(WinAnimationCoroutine());
    }

    IEnumerator WinAnimationCoroutine()
    {
        animator.enabled = false;
        raysSprite.transform.localScale = Vector2.zero;
        raysSprite.gameObject.SetActive(true);
        raysSprite.transform.DOScale(Vector3.one * 2, 2f).SetEase(Ease.InOutCubic);
        raysSprite.transform.DOLocalRotate(new Vector3(0,0,-360), 7f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        grapeSprite.transform.DOLocalMoveY(1, 2f).SetEase(Ease.InOutQuint);

        yield return new WaitForSeconds(5);
    }
}
