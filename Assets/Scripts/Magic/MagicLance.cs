using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicLance : Magic
{
    [SerializeField] ParticleSystem particle = default;
    [SerializeField] GradiantSet gradiantSet = default;

    private Transform trackTarget;
    private bool isExcute = false;


    private void Update()
    {
        if (!isExcute)
        {
            OnChaseToTarget(trackTarget);
            transform.forward = trackTarget.forward;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OnApplyDamage(other.gameObject, 10);
    }


    public override void OnGenerate(DataVisual data, Transform origin)
    {
        this.data = data;
        trackTarget = origin;
        transform.position = origin.position;

        var col = particle.colorOverLifetime;
        col.color = gradiantSet.GetGradient(data.Attribute);
    }
    public override void OnExcute(Vector3 expect)
    {
        isExcute = true;
        OnForceToRigidWithLifeTime(expect);
    }
}
