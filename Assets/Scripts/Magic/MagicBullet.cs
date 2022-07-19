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
        UpdateChaseIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerBranch(other, Data.Value);
    }


    protected override void Generate(DataVisual data, Transform origin)
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
}
