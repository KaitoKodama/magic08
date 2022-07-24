using System.Collections;
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
        OnTriggerActor(other, Data.Value, OnTriggerActorCompleted);
        OnTriggerField(other, OnTriggerFieldCompleted);
    }


    protected override void Excute(Vector3 expect)
    {
        SetRigidVelocity(expect);
    }


    private void OnTriggerActorCompleted()
    {
        SetDamageBox(transform.position, Data.Value);
        SetHitEffect();
    }
    private void OnTriggerFieldCompleted()
    {
        SetBreakEffect();
    }
}
