using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleArrow : Magic
{
    [SerializeField] MagicCircle magicCirclePrafab = default;


    private void Update()
    {
        UpdateChaseIfNotExcute();
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerField(other, OnTriggerFieldCompleted);
    }


    private void OnTriggerFieldCompleted()
    {
        var obj = Instantiate(magicCirclePrafab.gameObject);
        var circle = obj.GetComponent<MagicCircle>();
        circle.OnGenerate(Owner, Data, transform);
        circle.OnExcute(default);

        SetBreakEffect();
    }
    protected override void Excute(Vector3 expect)
    {
        SetRigidVelocity(expect);
    }
}
