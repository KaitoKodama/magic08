using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicLance : Magic
{
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GradiantSet gradiantSet = default;


    private void Update()
    {
        if (!IsExcute)
        {
            OnLerpToTarget();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IApplyDamageTrigger(other);
    }


    protected override void Generate(CharacterType character, DataVisual data, Transform origin)
    {
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
    }
    protected override void Excute(Vector3 expect)
    {
        transform.forward = expect;
        OnForceToRigidWithLifeTime(expect);
    }
    protected override void Destroy()
    {
        InstantinateResorces("Hit02");
        Destroy(this.gameObject);
    }
}
