using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicExplodeArrow : Magic
{
    [SerializeField] MagicExplode magicExplodePrafab = default;
    private Rigidbody rigid;
    private Transform forceTarget;
    private float riseSpeed = 3f;


    private void Update()
    {
        UpdateChaseIfNotExcute();
    }
    private void FixedUpdate()
    {
        if (IsExcute && forceTarget != null && !rigid.useGravity)
        {
            float distance = Vector3.Distance(transform.position, forceTarget.position);
            if (distance <= 2f || transform.position.y >= 3f)
            {
                rigid.useGravity = true;
            }
            else
            {
                var target = forceTarget.position;
                target.y += 5f;
                var force = (target - transform.position).normalized;
                rigid.AddForce(force * Time.fixedDeltaTime * riseSpeed);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerField(other);
    }


    protected override void Excute(Vector3 expect)
    {
        rigid = GetComponent<Rigidbody>();

        Transform targetform = null;
        float closet = float.MaxValue;
        var targets = GameObject.FindGameObjectsWithTag(GetTagFromObjectOfType());
        if (targets != null)
        {
            foreach (var target in targets)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance <= closet)
                {
                    closet = distance;
                    targetform = target.transform;
                }
            }
        }
        if (targetform != null) forceTarget = targetform;

        SetRigidVelocity(expect, 2);
    }

    protected override void OnTriggerFieldCompleted()
    {
        CloningGenerate<MagicExplode>(magicExplodePrafab.gameObject);
        SetBreakEffect();
        Destroy(this.gameObject);
    }


    private string GetTagFromObjectOfType()
    {
        string tag;
        if (Owner.GetType() == typeof(Player)) tag = "Enemy";
        else tag = "Player";
        return tag;
    }
}
