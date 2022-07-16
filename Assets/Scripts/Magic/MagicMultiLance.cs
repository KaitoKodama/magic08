using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMultiLance : Magic
{
    [SerializeField] Transform multiParent = default;
    [SerializeField] MonoTrigger[] triggers = default;
    [SerializeField] ParticleSystem[] particles = default;

    private Transform trackTarget;
    private bool isExcute = false;


    private void Update()
    {
        if (!isExcute)
        {
            OnChaseToTarget(trackTarget);
            transform.forward = trackTarget.forward;
        }
        multiParent.rotation = multiParent.rotation * Quaternion.Euler(0, 0, 1);
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

        foreach (var trigger in triggers)
        {
            trigger.AddListner((other) => { OnApplyDamage(other.gameObject, 10); });
        }
        SetSimulateSpeed(5);
    }
    public override void OnExcute(Vector3 expect)
    {
        isExcute = true;
        SetSimulateSpeed(2);
        OnForceToRigidWithLifeTime(expect);
    }


    private void SetSimulateSpeed(float speed)
    {
        foreach (var particle in particles)
        {
            var pt = particle.main;
            pt.simulationSpeed = speed;
        }
    }
}
