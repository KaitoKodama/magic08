using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMultiLance : Magic
{
    [SerializeField] Transform multiParent = default;
    [SerializeField] MonoTrigger[] triggers = default;
    [SerializeField] ParticleSystem[] particles = default;


    private void Update()
    {
        if (!IsExcute)
        {
            OnLerpToTarget();
        }
        multiParent.rotation = multiParent.rotation * Quaternion.Euler(0, 0, 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        IApplyDamageTrigger(other);
    }


    protected override void Generate(CharacterType character, DataVisual data, Transform origin)
    {
        foreach (var trigger in triggers)
        {
            trigger.AddTriggerEnterListner((other) =>
            {
                var target = other.GetComponent<IApplyDamage>();
                target?.ApplyDamage(this, character, data.Value / 4);
            });
        }
        SetSimulateSpeed(8);
    }
    protected override void Excute(Vector3 expect)
    {
        SetSimulateSpeed(5);
        transform.forward = expect;
        OnForceToRigidWithLifeTime(expect, 3);
    }
    protected override void Destroy()
    {
        InstantinateResorces("Hit01");
        Destroy(this.gameObject);
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
