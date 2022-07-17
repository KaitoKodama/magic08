using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObject/ActorData")]
public class ActorData : ScriptableObject
{
    [SerializeField] CharacterType character = CharacterType.Enemy;
    [SerializeField] float maxHp = 100;
    [SerializeField] float maxMp = 50;
    [SerializeField] float refillSpeed = 1;
    [SerializeField] string ownerName = default;
    [SerializeField] int level = 1;


    public CharacterType CharacterType => character;
    public float MaxHP => maxHp;
    public float MaxMP => maxMp;
    public float RefillSpeed => refillSpeed;
    public string OwnerName => ownerName;
    public int Level => level;
}
