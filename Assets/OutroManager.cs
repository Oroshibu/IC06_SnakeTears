using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class OutroManager : MonoBehaviour
{
    public Image transi;
    public TMPro.TextMeshProUGUI outroText;
    public TMPro.TextMeshProUGUI outroText2;
    public TMPro.TextMeshProUGUI outroText3;
    public RectTransform frescRect;

    bool canSkip;

    private void Start()
    {
        outroText.color = new Color(1, 1, 1, 0);
        outroText2.color = new Color(1, 1, 1, 0);
        outroText3.color = new Color(1, 1, 1, 0);

        StartCoroutine(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        Audio_Manager.i.PlayMusic(1);
        Audio_Manager.i.MusicFadeIn(1);
        
        yield return transi.DOFade(0, 2).WaitForCompletion();
        yield return outroText.DOFade(1, 2).WaitForCompletion();
        yield return new WaitForSeconds(3);
        frescRect.DOAnchorPosY(-265, 12f).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3);
        yield return outroText.DOFade(0, 2).WaitForCompletion();
        yield return new WaitForSeconds(2);
        yield return outroText2.DOFade(1, 2).WaitForCompletion();
        canSkip = true;
        yield return outroText3.DOFade(1, 2).WaitForCompletion();
        
    }
    
    void Skip()
    {
        if (canSkip)
        {
            canSkip = false;
            StartCoroutine(SkipCoroutine());
        }
    }

    IEnumerator SkipCoroutine(){
        yield return transi.DOFade(1, 2).WaitForCompletion();
        yield return new WaitForSeconds(.5f);
        Scene_Manager.i.LoadScene(1);
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Skip();
        }
    }
}
