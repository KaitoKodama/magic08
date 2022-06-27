using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class DataVisual : MagicData
{
    [SerializeField] Magic prefab = default;
    [SerializeField] int id = default;
    [SerializeField] float requireMP = default;
    [SerializeField] string explain = default;
    [SerializeField] MagicGrade grade = default;
    [SerializeField] MagicAttribute attribute = default;

    public Magic Prefab => prefab;
    public int ID => id;
    public float RequireMP => requireMP;
    public string Explain => explain;
    public MagicGrade Grade => grade;
    public MagicAttribute Attribute => attribute;

    public void SetVisualStatement(string nameJa, string nameEn, float requireMP, string explain, 
        MagicGrade grade, MagicAttribute attribute, int id)
    {
        this.nameJa = nameJa;
        this.nameEn = nameEn;
        this.requireMP = requireMP;
        this.explain = explain;
        this.grade = grade;
        this.attribute = attribute;
        this.id = id;
    }
}
