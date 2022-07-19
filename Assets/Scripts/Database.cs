using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;
using CMN;

public class Database : MonoBehaviour
{
    [SerializeField] DataGrade[] gradeset = default;
    [SerializeField] DataAttribute[] attributeset = default;
    [SerializeField] DataVisual[] visualset = default;
    private DataGrade[] gradesets;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public static Database instance;
    public DataGrade[] GetGradeset()
    {
        return gradeset;
    }
    public DataAttribute[] GetAttributeset(MagicGrade grade)
    {
        var visualTmp = visualset.Where(el => el.Grade == grade);
        var attributeTmp = new List<DataAttribute>(attributeset.Length);
        foreach (var attribute in attributeset)
        {
            foreach (var visual in visualTmp)
            {
                if (attribute.Attribute == visual.Attribute)
                {
                    attributeTmp.Add(attribute);
                    break;
                }
            }
        }
        return attributeTmp.ToArray();
    }
    public DataVisual[] GetVisualset(MagicGrade grade, MagicAttribute attribute)
    {
        var tmp = visualset.Where(el => el.Grade == grade && el.Attribute == attribute);
        return tmp.ToArray();
    }
}
