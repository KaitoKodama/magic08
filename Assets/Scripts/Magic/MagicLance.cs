using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicLance : Magic
{
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GradiantSet gradiantSet = default;
    private int triggerCount = 0;
    private int triggerMax = 3;


    private void Update()
    {
        UpdateLerpIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActor(other);
        OnTriggerField(other);
    }


    protected override void OnTriggerActorCompleted(Actor actor)
    {
        actor.ApplyDamage(transform, Data.Value);
        SetDamageBox(transform.position, Data.Value);
        SetHitEffect();

        triggerCount++;
        if (triggerCount >= triggerMax)
        {
            Destroy(this.gameObject);
        }
    }
    protected override void OnTriggerFieldCompleted()
    {
        SetBreakEffect();
        Destroy(this.gameObject);
    }
    protected override void Generate(DataVisual data, Transform origin)
    {
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
    }
    protected override void Excute(Vector3 expect)
    {
        transform.forward = expect;
        SetRigidVelocity(expect);
    }
}
