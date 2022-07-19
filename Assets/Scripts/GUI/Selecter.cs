using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicContext;
using DG.Tweening;

public class Selecter : MonoBehaviour
{
    [SerializeField] GameObject selecterPrefab = default;
    [SerializeField] Transform selecterParent = default;

    private List<SelecterGrid> magicGridList = new List<SelecterGrid>();
    private MagicData selectedData;
    private MagicGrade grade;
    private MagicAttribute attribute;
    private float createDegZ = 45f;
    private int loopLength = 8;


    //------------------------------------------
    // Unityランタイム
    //------------------------------------------
    private void Start()
    {
        float degZ = 0f;
        for (int i = 0; i < loopLength; i++)
        {
            var obj = Instantiate(selecterPrefab, selecterParent);
            var btn = obj.GetComponent<SelecterGrid>();
            magicGridList.Add(btn);
            btn.Init(this, degZ);
            degZ += createDegZ;
        }
        SetGridContents();
        gameObject.SetActive(false);
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public void GridDataReciever(MagicData data)
    {
        this.selectedData = data;
    }
    public void SetGridContents(MagicData[] datas = null)
    {
        MagicData[] expectset = (datas == null) ? Database.instance.GetGradeset() : datas;
        foreach (var el in magicGridList)
        {
            el.ResetMagicData();
        }
        for (int i = 0; i < magicGridList.Count; i++)
        {
            if (i >= expectset.Length)
            {
                magicGridList[i].SetMagicData("戻る", "Back");
                break;
            }
            magicGridList[i].SetMagicData(expectset[i]);
        }
    }
    public void SetGridContentsOrGenerate(Action<DataVisual> generateback)
    {
        if (selectedData != null)
        {
            var type = selectedData.GetType();
            if (type == typeof(DataGrade))
            {
                var tmp = selectedData as DataGrade;
                this.grade = tmp.Grade;
                SetGridContents(Database.instance.GetAttributeset(grade));
            }
            else if (type == typeof(DataAttribute))
            {
                var tmp = selectedData as DataAttribute;
                this.attribute = tmp.Attribute;
                SetGridContents(Database.instance.GetVisualset(grade, attribute));
            }
            else if (type == typeof(DataVisual))
            {
                generateback(selectedData as DataVisual);
            }
        }
        else SetGridContents();
    }
    public void SetSelecterState(bool isHolding)
    {
        gameObject.SetActive(isHolding);
    }
}
