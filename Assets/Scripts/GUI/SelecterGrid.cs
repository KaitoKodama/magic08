using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelecterGrid : MonoBehaviour
{
    [SerializeField] RectTransform boxrect = default;
    [SerializeField] Text textJa = default;
    [SerializeField] Text textEn = default;

    private Selecter selecter;
    private MagicData data;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnableSelecter"))
        {
            OnGridHovered(1f, 0.1f, -2f);
            selecter.GridDataReciever(data);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnableSelecter"))
        {
            OnGridHovered(0.3f, 0.09f, 0f);
        }
    }


    public void Init(Selecter selecter, float degZ)
    {
        transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, degZ));
        this.selecter = selecter;
    }
    public void SetMagicData(MagicData data)
    {
        this.data = data;
        gameObject.SetActive(true);
        SetTextScramble(data.NameJa, data.NameEn);
        OnGridHovered(0.3f, 0.09f, 0f);
        SetGridLocate(4f, 0f);
    }
    public void SetMagicData(string ja, string en)
    {
        this.data = null;
        gameObject.SetActive(true);
        SetTextScramble(ja, en);
        SetGridLocate(4f, 0f);
    }
    public void ResetMagicData()
    {
        textJa.text ="";
        textEn.text = "";
        gameObject.SetActive(false);
    }


    private void OnGridHovered(float alpha, float scale, float z)
    {
        textJa.color = new Color(1, 1, 1, alpha);
        textEn.color = new Color(1, 1, 1, alpha);
        boxrect.DOScale(scale, 0.5f);
        boxrect.DOLocalMoveZ(z, 0.5f);
    }
    private void SetTextScramble(string ja, string en)
    {
        textJa.DOText(ja, 0.3f, true, ScrambleMode.All).SetEase(Ease.Linear);
        textEn.DOText(en, 0.3f, true, ScrambleMode.All).SetEase(Ease.Linear);
    }
    private void SetGridLocate(float from, float to)
    {
        var pos = transform.localPosition;
        pos.z = from;
        transform.localPosition = pos;
        transform.DOLocalMoveZ(to, 0.3f);
    }
}
