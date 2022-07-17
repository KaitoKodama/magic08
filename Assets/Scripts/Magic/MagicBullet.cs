using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicBullet : Magic
{
    [SerializeField] ParticleSystem core = default;
    [SerializeField] ParticleSystem ring = default;
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
        var co = core.colorOverLifetime;
        var ri = ring.colorOverLifetime;
        var grad = gradiantSet.GetGradient(data.Attribute);
        co.color = grad;
        ri.color = grad;
    }
    protected override void Excute(Vector3 expect)
    {
        OnForceToRigidWithLifeTime(expect);
    }
    protected override void Destroy()
    {
        InstantinateResorces("Hit01");
        Destroy(this.gameObject);
    }
}
