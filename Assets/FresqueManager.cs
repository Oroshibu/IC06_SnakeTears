using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FresqueManager : MonoBehaviour
{
    public Image transi;
    public TMPro.TextMeshProUGUI credits1;
    public TMPro.TextMeshProUGUI credits2;
    public RectTransform frescRect;
    public HorizontalLayoutGroup layoutGroup;
    public GameObject texts;
    //public List<RectTransform> focusPoints;
    
    public List<float> focusPoints;
    private List<TMPro.TextMeshProUGUI> textsList;

    private void Start()
    {
        credits1.color = new Color(1, 1, 1, 0);
        credits2.color = new Color(1, 1, 1, 0);
        
        //Time.timeScale = 10;
        textsList = new List<TMPro.TextMeshProUGUI>();
        foreach(var txt in texts.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            txt.color = new Color(1, 1, 1, 0);
            textsList.Add(txt);
        }
        frescRect.anchoredPosition = new Vector2(focusPoints[0], frescRect.anchoredPosition.y);
        StartCoroutine(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(1);
        
        yield return credits1.DOFade(1, 1).WaitForCompletion();
        yield return new WaitForSeconds(1);
        yield return credits1.DOFade(0, 1).WaitForCompletion();
        
        yield return credits2.DOFade(1, 1).WaitForCompletion();
        yield return new WaitForSeconds(1);
        yield return credits2.DOFade(0, 1).WaitForCompletion();
        
        yield return transi.DOFade(0, 2).WaitForCompletion();
        for (int i = 1; i < focusPoints.Count; i++)
        {
            textsList[i - 1].DOFade(1, 3f).SetEase(Ease.OutSine);
            yield return new WaitForSeconds(6f);
            textsList[i - 1].DOFade(0, 8f).SetEase(Ease.InCubic);
            frescRect.DOAnchorPosX(focusPoints[i], 12f).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(12 - 3f);
        }
        textsList[focusPoints.Count - 1].DOFade(1, 3f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(6f);
        yield return transi.DOFade(1, 6).WaitForCompletion();
        Scene_Manager.i.LoadScene(1);
    }
}
