using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public abstract class Magic : MonoBehaviour
{
    protected DataVisual data;

    public DataVisual Data => data;
    public abstract void OnGenerate(DataVisual data, Transform origin);
    public abstract void OnExcute(Vector3 expect);


    protected void OnChaseToTarget(Transform target, float speed = 3f)
    {
        var lerp = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
        transform.position = lerp;
    }
    protected void OnForceToRigidWithLifeTime(Vector3 expect, float speed = 5f, float lifeTime = 10f)
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = expect * speed;
        Destroy(this.gameObject, lifeTime);
    }
    protected void OnApplyDamage(GameObject other, float damage)
    {
        var target = other.GetComponent<IApplyDamage>();
        if (target != null)
        {
            target.ApplyDamage(damage);
        }
    }
}
