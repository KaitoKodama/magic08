using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] float hp = default;
    [SerializeField] float mp = default;
    [SerializeField] string enemyName = default;
    [SerializeField, TextArea] string explain = default;
    [SerializeField] List<Magic> magicList = default;

    public float HP => hp;
    public float MP => mp;
    public string EnemyName => enemyName;
    public string Explain => explain;
    public List<Magic> MagicList => magicList;
}
