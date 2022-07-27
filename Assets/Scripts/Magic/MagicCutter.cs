using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public class MagicCutter : Magic
{
    [SerializeField] ParticleSystem[] cores = default;
    [SerializeField] GradiantSet gradiantSet = default;
    private Rigidbody rigid;
    private Transform forceTarget;
    [SerializeField] private float forceSpeed = 5f;
    private float time = 0;
    private float forceTime = 5f;
    private bool isForceEnd = false;


    private void Update()
    {
        UpdateChaseIfNotExcute();
        if (IsExcute && forceTarget != null && !isForceEnd)
        {
            time += Time.deltaTime;
            if (time >= forceTime)
            {
                isForceEnd = true;
                rigid.useGravity = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (forceTarget != null)
        {
            var target = forceTarget.position;
            var force = (target - transform.position).normalized;
            rigid.AddForce(force * Time.fixedDeltaTime * forceSpeed);
            transform.LookAt(forceTarget);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerActor(other);
        OnTriggerField(other);
    }


    protected override void Generate(DataVisual data, Transform origin)
    {
        rigid = GetComponent<Rigidbody>();
        foreach (var core in cores)
        {
            var co = core.colorOverLifetime;
            var grad = gradiantSet.GetGradient(data.Attribute);
            co.color = grad;
        }
    }
    protected override void Excute(Vector3 expect)
    {
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
        rigid.velocity = expect * 5f;
    }


    protected override void OnTriggerActorCompleted(Actor actor)
    {
        actor.ApplyDamage(transform, Data.Value);
        SetHitEffect();
        Destroy(this.gameObject);
    }
    protected override void OnTriggerFieldCompleted()
    {
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
