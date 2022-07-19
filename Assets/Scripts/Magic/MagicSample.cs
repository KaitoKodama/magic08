using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSample : Magic
{

    private void Update()
    {
        UpdateChaseIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerBranch(other, Data.Value);
    }


    protected override void Excute(Vector3 expect)
    {
        OnForceToRigidWithLifeTime(expect);
    }
}
