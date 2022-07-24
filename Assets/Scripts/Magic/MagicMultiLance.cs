using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMultiLance : Magic
{
    [SerializeField] Transform multiParent = default;
    [SerializeField] ParticleSystem[] particles = default;


    private void Update()
    {
        UpdateLerpIfNotExcute();
        multiParent.rotation = multiParent.rotation * Quaternion.Euler(0, 0, 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActor(other, Data.Value, OnTriggerActorCompleted);
        OnTriggerField(other, OnTriggerFieldCompleted);
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        SetSimulateSpeed(8);
    }
    protected override void Excute(Vector3 expect)
    {
        SetSimulateSpeed(5);
        transform.forward = expect;
        SetRigidVelocity(expect, 3f);
    }


    private void SetSimulateSpeed(float speed)
    {
        foreach (var particle in particles)
        {
            var pt = particle.main;
            pt.simulationSpeed = speed;
        }
    }
    private void OnTriggerActorCompleted()
    {
        SetDamageBox(transform.position, Data.Value);
        SetHitEffect();
    }
    private void OnTriggerFieldCompleted()
    {
        SetBreakEffect();
    }
}
