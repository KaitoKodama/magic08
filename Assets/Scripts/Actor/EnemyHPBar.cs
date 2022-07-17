using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] CanvasGroup group = default;
    [SerializeField] Image fillHp = default;
    [SerializeField] Image fillMp = default;
    [SerializeField] Text textHp = default;
    [SerializeField] Text textMp = default;
    [SerializeField] Text textDetail = default;
    private Transform camform;
    private bool isAlmostZero = false;


    private void Start()
    {
        camform = Camera.main.transform;
    }
    private void Update()
    {
        if (camform != null)
        {
            var pos = camform.position;
            transform.LookAt(pos, Vector3.up);
        }
    }


    public void UpdateStatePanel(ActorData data, float hp, float mp)
    {
        textDetail.text = $"Lv.{data.Level} : {data.OwnerName}";
        SetFillAndText(ref fillHp, ref textHp, hp, data.MaxHP);
        SetFillAndText(ref fillMp, ref textMp, mp, data.MaxMP);

        if (Mathf.Approximately(hp, 0) && !isAlmostZero)
        {
            group.DOFade(0, 1f);
            isAlmostZero = true;
        }
    }

    private void SetFillAndText(ref Image fill, ref Text text, float current, float max)
    {
        float clampHP = current / max;
        float lerp = Mathf.Lerp(fill.fillAmount, clampHP, Time.deltaTime);
        fill.fillAmount = lerp;
        text.text = $"HP : {((int)lerp)}/{max}";
    }
}
