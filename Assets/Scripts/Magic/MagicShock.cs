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


    protected override void Generate(DataVisual data, Transform origin)
    {
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
    }
    protected override void Excute(Vector3 expect)
    {
        OnForceToRigidWithLifeTime(expect);
    }
}
