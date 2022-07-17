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
        if (!IsExcute)
        {
            OnChaseToTarget();
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
        OnForceToRigidWithLifeTime(expect);
    }
    protected override void Destroy()
    {
        InstantinateResorces("Hit04");
        Destroy(this.gameObject);
    }
}
