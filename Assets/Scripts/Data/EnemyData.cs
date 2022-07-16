using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] float maxHp = 100;
    [SerializeField] float maxMp = 50;
    [SerializeField] float refillSpeed = 1;
    [SerializeField] string enemyName = default;
    [SerializeField, TextArea] string explain = default;
    [SerializeField] List<DataVisual> visualList = default;

    public float MaxHP => maxHp;
    public float MaxMP => maxMp;
    public float RefillSpeed => refillSpeed;
    public string EnemyName => enemyName;
    public string Explain => explain;
    public List<DataVisual> VisualList => visualList;
}
