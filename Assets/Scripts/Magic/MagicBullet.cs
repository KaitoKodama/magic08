using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicBullet : Magic
{
    [SerializeField] ParticleSystem core = default;
    [SerializeField] ParticleSystem ring = default;
    [SerializeField] GradiantSet gradiantSet = default;

    private Transform trackTarget;
    private bool isExcute = false;


    private void Update()
    {
        if (!isExcute)
        {
            OnChaseToTarget(trackTarget);
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

        var co = core.colorOverLifetime;
        var ri = ring.colorOverLifetime;
        var grad = gradiantSet.GetGradient(data.Attribute);
        co.color = grad;
        ri.color = grad;
    }
    public override void OnExcute(Vector3 expect)
    {
        isExcute = true;
        OnForceToRigidWithLifeTime(expect);
    }
}
