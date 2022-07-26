using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicExplode : Magic
{
    private int count;
    private float time;
    private int triggerCount = 3;
    private float destroyTime = 2f;


    private void OnTriggerEnter(Collider other)
    {
        if (count <= triggerCount)
        {
            OnTriggerActor(other);
        }
    }
    private void Update()
    {
        if (IsExcute)
        {
            time += Time.deltaTime;
            if (time >= destroyTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    protected override void OnTriggerActorCompleted(Actor actor)
    {
        actor.ApplyDamage(transform, Data.Value);
        SetDamageBox(transform.position, Data.Value);
        count++;
    }
}
