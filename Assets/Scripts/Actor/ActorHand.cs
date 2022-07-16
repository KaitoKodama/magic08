using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHand : MonoBehaviour
{
    [SerializeField] HandType handType = default;
    public HandType HandType => handType;
}
