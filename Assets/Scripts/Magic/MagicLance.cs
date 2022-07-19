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
        UpdateLerpIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerBranch(other, Data.Value);
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
    }
    protected override void Excute(Vector3 expect)
    {
        transform.forward = expect;
        OnForceToRigidWithLifeTime(expect);
    }
}
