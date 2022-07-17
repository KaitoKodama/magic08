using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicContext;
using DG.Tweening;

public class UIMagicSelecter : MonoBehaviour
{
    [SerializeField] GameObject selecterPrefab = default;
    [SerializeField] Transform selecterParent = default;
    [SerializeField] LogTracker logTracker = default;

    private List<UIMagicGrid> magicGridList = new List<UIMagicGrid>();
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
            var btn = obj.GetComponent<UIMagicGrid>();
            magicGridList.Add(btn);
            btn.Init(this, degZ);
            degZ += createDegZ;
        }
        SetGridContents(Database.instance.GetGradeset());
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public void GridDataReciever(MagicData data)
    {
        this.selectedData = data;
    }
    public void RequestSwapGridContent(Transform origin)
    {
        if (selectedData != null)
        {
            var type = selectedData.GetType();
            if (type == typeof(DataGrade))
            {
                SetDatatypeGrade();
            }
            else if (type == typeof(DataAttribute))
            {
                SetDatatypeAttribute();
            }
            else if (type == typeof(DataVisual))
            {
                SetDatatypeVisual(origin);
            }
        }
        else SetGridContents(Database.instance.GetGradeset());
    }


    //------------------------------------------
    // 内部共有関数
    //------------------------------------------
    private void SetGridContents(MagicData[] expectData)
    {
        foreach (var el in magicGridList)
        {
            el.ResetMagicData();
        }
        for (int i = 0; i < magicGridList.Count; i++)
        {
            if (i >= expectData.Length)
            {
                magicGridList[i].SetMagicData("戻る", "Back");
                break;
            }
            magicGridList[i].SetMagicData(expectData[i]);
        }
    }
    private void SetDatatypeGrade()
    {
        var tmp = selectedData as DataGrade;
        grade = tmp.Grade;
        SetGridContents(Database.instance.GetAttributeset(grade));
    }
    private void SetDatatypeAttribute()
    {
        var tmp = selectedData as DataAttribute;
        attribute = tmp.Attribute;
        SetGridContents(Database.instance.GetVisualset(grade, attribute));
    }
    private void SetDatatypeVisual(Transform origin)
    {
        var tmp = selectedData as DataVisual;
        if (Locator<Player>.I != null)
        {
            string log;
            if (Locator<Player>.I.EnableMagic == null)
            {
                Locator<Player>.I.GenerateMagic(origin, tmp);
                log = $"{Locator<Player>.I.EnableMagic.Data.NameJa}の発動を完了しました";
            }
            else
            {
                log = $"連続で発動準備は行えません\n{Locator<Player>.I.EnableMagic.Data.NameJa}の発動を完了させてください";
            }
            logTracker.SetLogText(log);
        }
        SetGridContents(Database.instance.GetGradeset());
    }
}
