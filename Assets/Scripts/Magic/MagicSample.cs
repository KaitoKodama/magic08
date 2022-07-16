using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSample : Magic
{
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
    }
    public override void OnExcute(Vector3 expect)
    {
        isExcute = true;
        OnForceToRigidWithLifeTime(expect);
    }
}
