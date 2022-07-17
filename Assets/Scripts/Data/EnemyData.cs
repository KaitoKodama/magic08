using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ActorData
{
    [SerializeField, TextArea] string explain = default;
    [SerializeField] List<DataVisual> visualList = default;

    public string Explain => explain;
    public List<DataVisual> VisualList => visualList;
}
