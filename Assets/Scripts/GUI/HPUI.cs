using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPUI : MonoBehaviour
{

    [Header("Null非許容")]
    [SerializeField] Image fillHp = default;
    [SerializeField] Image fillMp = default;
    [SerializeField] Text textHp = default;
    [SerializeField] Text textMp = default;

    [Header("Null許容")]
    [SerializeField] CanvasGroup group = default;
    [SerializeField] Text textDetail = default;

    private bool isAlmostZero = false;



    public void UpdateStatePanel(ActorData data, float hp, float mp)
    {
        if (textDetail != null)
        {
            textDetail.text = $"Lv.{data.Level} : {data.OwnerName}";
        }

        SetFillAndText(fillHp, textHp, hp, data.MaxHP);
        SetFillAndText(fillMp, textMp, mp, data.MaxMP);

        if (group != null)
        {
            if (Mathf.Approximately(hp, 0) && !isAlmostZero)
            {
                group.DOFade(0, 1f);
                isAlmostZero = true;
            }
        }
    }

    private void SetFillAndText(Image fill, Text text, float current, float max)
    {
        float clampHP = current / max;
        float lerp = Mathf.Lerp(fill.fillAmount, clampHP, Time.deltaTime);
        fill.fillAmount = lerp;
        text.text = $"HP : {(int)(lerp * max)}/{max}";
    }
}
