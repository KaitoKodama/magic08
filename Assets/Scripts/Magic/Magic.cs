using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Magic : MonoBehaviour
{
    protected DataVisual data;
    public abstract void OnGenerate(DataVisual data, Transform origin);
    public abstract void OnExcute(Transform expect);
}
