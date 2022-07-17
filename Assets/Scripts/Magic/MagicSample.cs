using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSample : Magic
{

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


    protected override void Excute(Vector3 expect)
    {
        OnForceToRigidWithLifeTime(expect);
    }
    protected override void Destroy()
    {
        InstantinateResorces("Hit03");
        Destroy(this.gameObject);
    }
}
