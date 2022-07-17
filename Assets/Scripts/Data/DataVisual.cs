using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class DataVisual : MagicData
{
    [SerializeField] Magic prefab = default;
    [SerializeField] float value = default;
    [SerializeField] float requireMP = default;
    [SerializeField] string explain = default;
    [SerializeField] MagicGrade grade = default;
    [SerializeField] MagicAttribute attribute = default;
    [SerializeField] bool edit = false;

    public Magic Prefab => prefab;
    public float RequireMP => requireMP;
    public float Value => value;
    public string Explain => explain;
    public MagicGrade Grade => grade;
    public MagicAttribute Attribute => attribute;

    public void SetVisualStatement(string nameJa, string nameEn, float requireMP, float value, string explain, 
        MagicGrade grade, MagicAttribute attribute)
    {
        this.nameJa = nameJa;
        this.nameEn = nameEn;
        this.value = value;
        this.requireMP = requireMP;
        this.explain = explain;
        this.grade = grade;
        this.attribute = attribute;
    }
}
