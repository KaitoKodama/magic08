using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : Magic
{
    private int count;
    private float time;
    private int triggerCount = 3;
    private float destroyTime = 2f;


    private void OnTriggerEnter(Collider other)
    {
        if (count <= triggerCount)
        {
            var target = other.gameObject.GetComponent<Actor>();
            if (target != null)
            {
                if (Owner.GetType() != target.GetType())
                {
                    SetDamageBox(target.transform.position, Data.Value);
                    SetHitEffect();
                    target.ApplyDamage(Data.Value);
                    count++;
                }
            }
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
}
