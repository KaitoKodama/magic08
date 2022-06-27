using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

[CreateAssetMenu(fileName = "DataGrade", menuName = "MagicData/DataGrade")]
public class DataGrade : MagicData
{
    [SerializeField] MagicGrade grade = default;

    public MagicGrade Grade => grade;
}
