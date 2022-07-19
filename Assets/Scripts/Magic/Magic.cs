using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicContext;

public abstract class Magic : MonoBehaviour
{
    private DataVisual data;
    private Actor owner;


    //------------------------------------------
    // 外部共有関数
    //------------------------------------------
    public DataVisual Data => data;
    public void OnGenerate(Actor owner, DataVisual data, Transform origin)
    {
        this.data = data;
        this.owner = owner;
        TrackTarget = origin;
        transform.position = TrackTarget.position;
        Generate(data, origin);
    }
    public void OnExcute(Vector3 expect)
    {
        IsExcute = true;
        Excute(expect);
    }


    //------------------------------------------
    // 継承先共有抽象関数
    //------------------------------------------
    protected virtual void Generate(DataVisual data, Transform origin) { }
    protected abstract void Excute(Vector3 expect);


    //------------------------------------------
    // 継承先共有関数 - 戻り値あり
    //------------------------------------------
    protected Transform TrackTarget { get; private set; }
    protected bool IsExcute { get; private set; } = false;
    protected bool IsSameTypeActor(Actor compare)
    {
        if (owner.GetType() == compare.GetType())
        {
            return true;
        }
        return false;
    }


    //------------------------------------------
    // 継承先共有関数 - 戻り値なし
    //------------------------------------------
    protected void UpdateLerpIfNotExcute(float speed = 3f)
    {
        if (!IsExcute)
        {
            var lerp = Vector3.Lerp(transform.position, TrackTarget.position, Time.deltaTime * speed);
            transform.position = lerp;
            transform.forward = TrackTarget.forward;
        }
    }
    protected void UpdateChaseIfNotExcute()
    {
        if (!IsExcute)
        {
            transform.position = TrackTarget.position;
            transform.forward = TrackTarget.forward;
        }
    }
    protected void OnForceToRigidWithLifeTime(Vector3 expect, float speed = 5f, float lifeTime = 10f)
    {
        var rigid = GetComponent<Rigidbody>();
        rigid.velocity = expect * speed;
        Destroy(this.gameObject, lifeTime);
    }
    protected void TriggerBranch(Collider col, float damage)
    {
        TryTriggerTreatOfActor(col, damage);
        if (IsExcute)
        {
            TryTriggerTreatOfField(col);
        }
    }


    //------------------------------------------
    // 内部共有関数 - 戻り値なし
    //------------------------------------------
    private void TryTriggerTreatOfActor(Collider col, float damage)
    {
        var target = col.gameObject.GetComponent<Actor>();
        if (target != null)
        {
            if (!IsSameTypeActor(target))
            {
                target.ApplyDamage(damage);
                InstanceateFromResorces("Prefabs/Hit01");
                if (target.GetComponent<Enemy>())
                {
                    Locator<PlayerInput>.I.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
                }
                Destroy(this.gameObject);
            }
        }
    }
    private void TryTriggerTreatOfField(Collider col)
    {
        if (col.gameObject.CompareTag("Field"))
        {
            InstanceateFromResorces("Prefabs/BreakEffect");
            if (owner.GetType() == typeof(Player) && owner.EnableMagic)
            {
                Locator<PlayerInput>.I.OnVivration(0.1f, PlayerInput.VivrateHand.Holding);
            }
            Destroy(this.gameObject);
        }
    }
    private void InstanceateFromResorces(string path)
    {
        var prefab = Resources.Load(path);
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
