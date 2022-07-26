using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : Magic
{
    private bool isEnter = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!isEnter)
        {
            OnTriggerActor(other);
        }
    }

    protected override void OnTriggerActorCompleted(Actor actor)
    {
        actor.ApplyDamage(transform, Data.Value);
        SetDamageBox(transform.position, Data.Value);
        Destroy(this.gameObject, 5f);
        isEnter = true;
    }
}
