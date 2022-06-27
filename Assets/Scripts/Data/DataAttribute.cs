using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

[CreateAssetMenu(fileName = "DataAttribute", menuName = "MagicData/DataAttribute")]
public class DataAttribute : MagicData
{
    [SerializeField] MagicAttribute attribute = default;

    public MagicAttribute Attribute => attribute;
}
