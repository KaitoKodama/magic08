using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuTracker : MonoBehaviour
{
    [SerializeField] Tracker menuTracker = default;

    void Update()
    {
        menuTracker.UpdatePosition();
        menuTracker.UpdateRotation();
    }
}
