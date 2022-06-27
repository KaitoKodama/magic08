using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MagicGrid : MonoBehaviour
{
    [SerializeField] Text textJa = default;
    [SerializeField] Text textEn = default;
    private MagicSelecter selecter;
    private MagicData data;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnableSelecter"))
        {
            SetTextAlpha(1f);
            selecter.GridDataReciever(data);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnableSelecter"))
        {
            SetTextAlpha(0.3f);
        }
    }


    public void Init(MagicSelecter selecter, float degZ)
    {
        SetTextAlpha(0.3f);
        transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, degZ));
        this.selecter = selecter;
    }
    public void SetMagicData(MagicData data)
    {
        this.data = data;
        gameObject.SetActive(true);
        SetTextScramble(data.NameJa, data.NameEn);
        SetGridEffect(4f, 0f);
    }
    public void SetMagicData(string ja, string en)
    {
        this.data = null;
        gameObject.SetActive(true);
        SetTextScramble(ja, en);
        SetGridEffect(4f, 0f);
    }
    public void ResetMagicData()
    {
        textJa.text ="";
        textEn.text = "";
        gameObject.SetActive(false);
    }
    private void SetTextAlpha(float alpha)
    {
        textJa.color = new Color(1, 1, 1, alpha);
        textEn.color = new Color(1, 1, 1, alpha);
    }
    private void SetTextScramble(string ja, string en)
    {
        textJa.DOText(ja, 0.3f, true, ScrambleMode.All).SetEase(Ease.Linear);
        textEn.DOText(en, 0.3f, true, ScrambleMode.All).SetEase(Ease.Linear);
    }
    private void SetGridEffect(float from, float to)
    {
        var pos = transform.localPosition;
        pos.z = from;
        transform.localPosition = pos;
        transform.DOLocalMoveZ(to, 0.3f);
    }
}
