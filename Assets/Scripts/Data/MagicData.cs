using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicData : ScriptableObject
{
    [SerializeField]
    protected string nameJa = default;
    [SerializeField]
    protected string nameEn = default;

    public string NameJa => nameJa;
    public string NameEn => nameEn;
}
