using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicShock : Magic
{
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GradiantSet gradiantSet = default;


    private void Update()
    {
        UpdateChaseIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActor(other, Data.Value, OnTriggerActorCompleted);
        OnTriggerField(other, OnTriggerFieldCompleted);
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
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
