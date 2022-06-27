using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagicContext;
using DG.Tweening;

public class MagicSelecter : MonoBehaviour
{
    [SerializeField] GameObject selecterPrefab = default;
    [SerializeField] Transform selecterParent = default;
    [SerializeField] LogTracker logTracker = default;

    private List<MagicGrid> magicGridList = new List<MagicGrid>();
    private List<Magic> resisterList = new List<Magic>();
    private MagicData prevData;
    private MagicData currentData;
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
            var btn = obj.GetComponent<MagicGrid>();
            magicGridList.Add(btn);
            btn.Init(this, degZ);
            degZ += createDegZ;
        }
        SetGridContents(Database.instance.GetGradeset());
    }


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public bool IsResisterExist { get { return resisterList.Count > 0; } }
    public void GridDataReciever(MagicData data)
    {
        this.currentData = data;
    }
    public void RequestExcute(Transform origin)
    {
        resisterList[0].OnExcute(origin);
        resisterList.Clear();
    }
    public void RequestSwapGridContent(Transform origin)
    {
        if (currentData != null)
        {
            var type = currentData.GetType();
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
        var tmp = currentData as DataGrade;
        grade = tmp.Grade;
        SetGridContents(Database.instance.GetAttributeset(grade));
    }
    private void SetDatatypeAttribute()
    {
        var tmp = currentData as DataAttribute;
        attribute = tmp.Attribute;
        SetGridContents(Database.instance.GetVisualset(grade, attribute));
    }
    private void SetDatatypeVisual(Transform origin)
    {
        if (resisterList.Count == 0)
        {
            var tmp = currentData as DataVisual;
            var magic = Instantiate(tmp.Prefab).GetComponent<Magic>();
            magic.OnGenerate(tmp, origin);
            resisterList.Add(magic);
            logTracker.SetLogText($"{tmp.NameEn}\n{tmp.NameJa}の発動準備を完了");
            prevData = tmp;
        }
        else
        {
            logTracker.SetLogText($"連続で発動準備は行えません\n{prevData.NameJa}の発動を完了させてください");
        }
        SetGridContents(Database.instance.GetGradeset());
    }
}
