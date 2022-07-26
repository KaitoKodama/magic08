﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSample : Magic
{
    private void Update()
    {
        UpdateChaseIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActor(other);
        OnTriggerField(other);
    }


    protected override void Excute(Vector3 expect)
    {
        SetRigidVelocity(expect);
    }


    protected override void OnTriggerActorCompleted(Actor actor)
    {
        actor.ApplyDamage(transform, Data.Value);
        SetDamageBox(transform.position, Data.Value);
        SetHitEffect();
    }
    protected override void OnTriggerFieldCompleted()
    {
        SetBreakEffect();
        Destroy(this.gameObject);
    }
}
