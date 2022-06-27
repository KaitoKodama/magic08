﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StaffTracker : MonoBehaviour
{
    [SerializeField] Tracker fwdTracker = default;
    [SerializeField] Tracker selecterTracker = default;
    [SerializeField] Tracker enableTracker = default;
    [SerializeField] CanvasGroup[] groups = default;
    [SerializeField] Transform outline = default;
    [SerializeField] Transform inner = default;
    [SerializeField] float selecterScale = 2f;


    private void Start()
    {
        var actor = GetComponentInParent<Actor>();
        actor.OnStaffHolingNotifyerHandler = OnStaffHoldingNotifyerReciever;

        foreach (var group in groups)
        {
            group.alpha = 0f;
        }
        outline.DOLocalRotate(new Vector3(0, 0, 360), 15f).SetEase(Ease.Linear).SetRelative().SetLoops(-1, LoopType.Incremental);
        inner.DOLocalRotate(new Vector3(0, 0, -360), 10f).SetRelative().SetLoops(-1, LoopType.Incremental);
    }
    private void Update()
    {
        fwdTracker.UpdatePosition();
        fwdTracker.UpdateRotation();

        selecterTracker.UpdatePosition();
        selecterTracker.UpdateRotation(Tracker.VecOp.Z, selecterScale);

        enableTracker.UpdatePosition();
        enableTracker.UpdateFixRotation(Tracker.VecOp.Z);
    }

    private void OnStaffHoldingNotifyerReciever(bool isHolding)
    {
        float alpha = isHolding ? 1f : 0f;
        foreach (var group in groups)
        {
            group.DOFade(alpha, 0.5f);
        }
    }
}
